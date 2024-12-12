using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    new public string name = "New item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    //this method can be overridden
    public virtual void Use() {
        //use the item -- drop it in the scene ??
        Debug.Log("Using " + name);
    }
}
