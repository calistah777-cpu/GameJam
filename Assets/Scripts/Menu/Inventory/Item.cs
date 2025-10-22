using UnityEngine;
using UnityEngine.UI; 

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;

    public virtual void UseItem() {
        Debug.Log("Using item " + Name);
    }

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite; //use GetComponent<SpriteRenderer>() if you don't have an image
        if(ItemPickupUIController.Instance != null) {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
    }
}
