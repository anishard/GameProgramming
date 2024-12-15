using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public LayerMask ground;

    public GameObject inventoryUI;
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;
    public static GameObject equippedInteractable;
    public Interactable focus;
    public static AudioClip equipClip;
    public static AudioClip dequipClip;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        equippedInteractable = null;
        equipClip = Array.Find(Game.audioClips, (e) => e.name == "Equip");
        dequipClip = Array.Find(Game.audioClips, (e) => e.name == "Dequip");
    }

    void Update()
    {
        if (Game.ClickDetected(false) && !InventoryIsOpen())
        {
            var other = InteractableDetected();

            if (other != null && Player.equipped == Equippable.None) // holding nothing
            {
                EquipInteractable(other);
            }
            else if (other != null && Player.IsTool(Player.equipped)) // holding tool
            {
                Player.DequipTool(Equippable.Interactable);
                EquipInteractable(other);
            }
            else if (Player.equipped == Equippable.Interactable)
            {
                DequipInteractable(other);
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            AddToInventory();
        }

        // OPEN OR CLOSE the inventory when the Inventory button is clicked
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        // if inventory is ALREADY OPEN and click outside of it, close it
        bool clickedOutsideInventory = Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
        if (clickedOutsideInventory && InventoryIsOpen()) // Only close inventory if it's open
        {
            ToggleInventory();
        }
    }

    public void AddToInventory()
    {
        Interactable newFocus = equippedInteractable?.GetComponent<Interactable>();

        if (newFocus == null) return;

        if (newFocus != focus)
        {
            if (focus != null) focus.OnDefocused();
            focus = newFocus;
        }

        newFocus.OnFocused();
        Player.equipped = Equippable.None;
        equippedInteractable = null;

        StartCoroutine(Game.PlayAudio("Inventory", 0.4f));
    }

    private static void EquipInteractable(GameObject other)
    {
        if (other == null) return;
        other.transform.parent = GameObject.Find("InteractableContainer").transform;
        other.transform.localPosition = Vector3.zero;
        Player.equipped = Equippable.Interactable;
        equippedInteractable = other;

        Game.audioSource.PlayOneShot(equipClip, 0.3f);
    }

    public static void DequipInteractable(GameObject other)
    {
        equippedInteractable.transform.parent = null;
        Player.equipped = Equippable.None;

        PlaceOnGround();

        equippedInteractable = null;

        if (other != null)
            EquipInteractable(other);

        if (other == null && !Player.IsTool(Player.equipped))
            Game.audioSource.PlayOneShot(dequipClip, 0.3f);
    }

    public static void DestroyInteractable()
    {
        var obj = equippedInteractable;
        equippedInteractable = null;
        Destroy(obj);

        Player.equipped = Equippable.None;
    }

    public static GameObject InteractableDetected()
    {
        Collider[] colliders = Physics.OverlapSphere(
            Player.activeArea.transform.position, 0.75f
        );

        return Array.Find(colliders, (c) =>
            c.CompareTag("Interactable")
            && !GameObject.ReferenceEquals(c.gameObject, equippedInteractable)
        )?.gameObject;
    }

    private static void PlaceOnGround()
    {
        Transform tr = equippedInteractable.transform;
        var collider = equippedInteractable.GetComponent<BoxCollider>();
        
        if (collider == null)
            throw new Exception("Interactable must have a box collider to be placed on the ground");

        Vector3 toGround = collider.bounds.center - new Vector3(0, collider.bounds.extents.y, 0);
        
        if (Physics.Raycast(tr.position, Vector3.down, out RaycastHit hit))
            tr.position -= new Vector3(0, toGround.y - hit.point.y, 0);
    }

    // check if the inventory is already open
    bool InventoryIsOpen()
    {
        return inventoryUI.activeSelf;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                if (inventory.items[i].itemAmount > 0)
                {
                    //Debug.Log("adding item to inventory");
                    slots[i].AddItem(inventory.items[i]);
                    slots[i].itemAmount.enabled = true;
                    slots[i].itemAmount.text = inventory.items[i].itemAmount.ToString("n0");
                }
                else
                {
                    slots[i].ClearSlot();
                    slots[i].itemAmount.enabled = false;
                }
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].itemAmount.enabled = false;
            }
        }
    }

    public void ToggleInventory()
    {
        bool setActive = !InventoryIsOpen();

        if (setActive) Clock.Pause();
        else Clock.Resume();

        inventoryUI.SetActive(setActive);
        Player.pauseMovement = setActive;

        UpdateUI();
    }
}
