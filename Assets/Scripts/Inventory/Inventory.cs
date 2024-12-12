using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }

    #endregion 
    
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 15;

    public List<Item> items = new List<Item>();

    
    public bool Add (Item item) {
        //stacking
        Item copyItem = Instantiate(item);
        if (!item.isDefaultItem) {
            if (items.Count >= space) {
                Debug.Log("Not enough room.");
                return false;
            }
            //for stacking
            for (int i=0; i<items.Count; i++) {
                if (items[i].name == item.name) {
                    items[i].itemAmount++;
                    Debug.Log("there are: " + items[i].itemAmount + " " + item.name);
                    if (onItemChangedCallback != null) {
                        onItemChangedCallback.Invoke();
                    }       
                    return true;
                }
            }
            //items.Add(item);
            //stacking
            items.Add(copyItem);
            if (onItemChangedCallback != null) {
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove(Item item) {
        items.Remove(item);
        if (onItemChangedCallback != null) {
            onItemChangedCallback.Invoke();
        }
    }
}
