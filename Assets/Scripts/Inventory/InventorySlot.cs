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

    void Start()
    {
        itemAmount.enabled = false;
        player = GameObject.Find("Player");
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        itemAmount.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        itemAmount.enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        StartCoroutine(Game.PlayAudio("Miss", 0.5f));
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (InventoryUI.equippedInteractable != null)
            {
                StartCoroutine(Game.PlayAudio("Miss", 0.5f));
            }
            else if (item.itemAmount >= 1)
            {
                item.Use();
                item.itemAmount--; //decrease the item amount each time an item is used
                itemAmount.text = item.itemAmount.ToString("n0"); //make sure the slot UI is updated
                //Instantiate the 3d prefab item back into the scene
                Vector3 playerPos = player.transform.position;
                Vector3 playerForward = player.transform.forward;
                string pathname = "InventorySprites/" + item.name;
                GameObject placeItemBack = (GameObject)Resources.Load("InventorySprites/" + item.name, typeof(GameObject));

                MeshRenderer collider = placeItemBack.GetComponent<MeshRenderer>();
                float collHeight = collider.bounds.size.y;
                float distInFront = 2f;
                Vector3 itemPosition = playerPos + playerForward * distInFront;
                itemPosition.y = collHeight / 2;

                //add the prefab back into the game at a position slightly in front of the player
                var parent = GameObject.Find("InteractableContainer").GetComponent<Transform>();
                var newItem = Instantiate(placeItemBack, Vector3.zero, Quaternion.identity, parent);
                newItem.transform.localPosition = Vector3.zero;
                Player.equipped = Equippable.Interactable;
                InventoryUI.equippedInteractable = newItem;

                if (item.itemAmount == 0)
                { //if decrease from 1 to 0, disable the slot UI text
                    ClearSlot();
                    itemAmount.enabled = false;
                }

                StartCoroutine(Game.PlayAudio("Inventory", 0.4f));
            }
        }
    }
}
