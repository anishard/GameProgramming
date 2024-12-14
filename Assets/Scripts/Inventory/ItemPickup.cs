using UnityEngine;

public class ItemPickup : Interactable {

    public Item item;

    public override void Interact() {
        
        base.Interact();
        // pick up the item
        PickUp();
    }

    public void PickUp() {
        //Add item to inventory
        bool wasPickedUp = Inventory.instance.Add(item);
        //remove item from scene
        if (wasPickedUp) {
            Destroy(gameObject);
        }
    }
}
