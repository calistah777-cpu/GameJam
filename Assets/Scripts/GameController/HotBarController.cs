using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarPanel;
    public GameObject slotPrefab;
    public int slotCount = 10;

    private ItemDictionary itemDictionary;

    private Key[] hotBarKeys;

    private void Awake() {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();
        hotBarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++) {
            hotBarKeys[i] = i < 9? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++) {
            if (Keyboard.current[hotBarKeys[i]].wasPressedThisFrame) {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index) {
        Slot slot = hotBarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot.currentItem != null) {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotBarItems() {
        List<InventorySaveData> hbData = new List<InventorySaveData>();
        foreach (Transform slotTransform in hotBarPanel.transform) {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null) {
                Item item = slot.currentItem.GetComponent<Item>();
                hbData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }
        return hbData;
    }

    public void SetHotBarItems(List<InventorySaveData> hotBarSaveData) {
        foreach(Transform child in hotBarPanel.transform) {
            Destroy(child.gameObject);  //clear inventory to avoid duplicates
        }

        for (int i = 0; i < slotCount; i++) {
            Instantiate(slotPrefab, hotBarPanel.transform);  //create new slots
        }

        foreach(InventorySaveData data in hotBarSaveData) {
            if (data.slotIndex < slotCount) {
                Slot slot = hotBarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null) {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}