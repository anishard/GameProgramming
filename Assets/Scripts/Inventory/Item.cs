using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    //for stacking items
    public int itemAmount;

    void Start() {
        itemAmount = 1;
    }

    //this method can be overridden
    public virtual void Use() {
        //use the item -- drop it in the scene ??
    }
}
