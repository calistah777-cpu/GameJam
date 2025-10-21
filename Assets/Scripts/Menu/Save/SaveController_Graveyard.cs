using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Cinemachine;
using System.Linq;

public class SaveController_Graveyard : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotBarController hotBarController;
    private Chest[] chests;

    void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents() 
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData2.json");
        Debug.Log(saveLocation);
        inventoryController = FindAnyObjectByType<InventoryController>();
        hotBarController = FindAnyObjectByType<HotBarController>();
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
        
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotBarSaveData = hotBarController.GetHotBarItems(),
            chestSaveData = GetChestState(),
            questProgressData = QuestController.Instance.activateQuests,
            handInQuestIDs = QuestController.Instance.handInQuestIDs
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private List<ChestSaveData> GetChestState() {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();

        foreach(Chest chest in chests) {
            ChestSaveData chestSaveData = new ChestSaveData {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotBarController.SetHotBarItems(saveData.hotBarSaveData);

            LoadChestState(saveData.chestSaveData);

            QuestController.Instance.LoadQuestProgress(saveData.questProgressData);
            QuestController.Instance.handInQuestIDs = saveData.handInQuestIDs;

        }
        else
        {
            SaveGame();

            inventoryController.SetInventoryItems(new List<InventorySaveData>());
            hotBarController.SetHotBarItems(new List<InventorySaveData>());
        }
    }

    private void LoadChestState(List<ChestSaveData> chestStates) {
        foreach(Chest chest in chests) {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if(chestSaveData != null) {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}

