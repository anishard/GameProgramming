using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemAmount;
    private GameObject player;

    Item item;

    // public delegate void OnItemChanged();
    // public OnItemChanged onItemChangedCallback;

    // public static Image icon;

    // void Awake() {
    //     // if instance is not yet set, set it and make it persistent between scenes
    //     if (icon == null)
    //     {
    //         icon = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         // if instance is already set and this is not the same object, destroy it
    //         if (this != icon) 
    //         { 
    //             Destroy(gameObject); 
    //         }
    //     }
    // }


    void Start() {
        itemAmount.enabled = false;
        player = GameObject.Find("Player");
    }

    public void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        itemAmount.enabled = true;
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        itemAmount.enabled = false;
    }

    public void OnRemoveButton() {
        Inventory.instance.Remove(item);
    }

    public void UseItem() {
        if (item != null) {
            if (item.itemAmount >= 1) {
                item.Use();
                item.itemAmount--; //decrease the item amount each time an item is used
                itemAmount.text = item.itemAmount.ToString("n0"); //make sure the slot UI is updated
                //Instantiate the 3d prefab item back into the scene
                Vector3 playerPos = player.transform.position;
                Vector3 playerForward = player.transform.forward;
                string pathname = "InventorySprites/Prefab_" + item.name;
                GameObject placeItemBack = (GameObject)Resources.Load("InventorySprites/Prefab_" + item.name, typeof(GameObject));
                
                MeshRenderer collider = placeItemBack.GetComponent<MeshRenderer>();
                float collHeight = collider.bounds.size.y;
                float distInFront = 2f;
                Vector3 itemPosition = playerPos + playerForward * distInFront;
                itemPosition.y = collHeight / 2;
                
                //add the prefab back into the game at a position slightly in front of the player
                Instantiate(placeItemBack, itemPosition, Quaternion.identity);
                if (item.itemAmount == 0) { //if decrease from 1 to 0, disable the slot UI text
                    ClearSlot();
                    itemAmount.enabled = false;
                }
            }
        }
    }
}
