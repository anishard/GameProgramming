using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake() {
        // if instance is not yet set, set it and make it persistent between scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // if instance is already set and this is not the same object, destroy it
            if (this != instance) 
            { 
                Destroy(gameObject); 
            }
        }
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
                return false;
            }
            //for stacking
            for (int i=0; i<items.Count; i++) {
                if (items[i].name == item.name) {
                    items[i].itemAmount++;
                    if (onItemChangedCallback != null) {
                        onItemChangedCallback.Invoke();
                    }       
                    return true;
                }
            }
            
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
        // stacking
        for (int i=0; i<items.Count; i++) {
            if (items[i].name == item.name) {
                items[i].itemAmount--;
            }
        }

        if (onItemChangedCallback != null) {
            onItemChangedCallback.Invoke();
        }
    }
}
