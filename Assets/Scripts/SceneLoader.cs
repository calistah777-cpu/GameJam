using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    private string saveLocation;

    private void Awake()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("[MainMenu] Save file path: " + saveLocation);
    }

    public void OnNewGame()
    {
        Debug.Log("[MainMenu] Starting new game...");

        if (File.Exists(saveLocation))
        {
            try
            {
                File.Delete(saveLocation);
                Debug.Log("[MainMenu] Old save file deleted.");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[MainMenu] Failed to delete old save: " + ex.Message);
            }
        }

        /*
        SaveData newSave = new SaveData { lastScene = "CemeteryScene" };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(newSave));*/

        //SceneManager.LoadScene("CemeteryScene");
        SceneManager.LoadScene("Intro");
    }

    public void OnLoadGame()
    {
        if (File.Exists(saveLocation))
        {
            string json = File.ReadAllText(saveLocation);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            if (!string.IsNullOrEmpty(saveData.lastScene))
            {
                SceneManager.LoadScene(saveData.lastScene);
            }
            else
            {
                Debug.LogWarning("[MainMenu] Save file invalid. Loading default scene.");
                SceneManager.LoadScene("CemeteryScene");
            }
        }
        else
        {
            Debug.LogWarning("[MainMenu] No save file found. Starting new game instead...");
            OnNewGame();
        }
    }

    public void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SaveData saveData = new SaveData { lastScene = currentScene };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        Debug.Log("[SceneLoader] Saved current scene: " + currentScene);
    }

    public void OnQuit()
    {
        Debug.Log("[MainMenu] Quitting game...");
        SceneManager.LoadScene("Menu");
        Application.Quit();
    }
}
