using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DepositDishes : MonoBehaviour
{
    Inventory inventory;
    public GameObject depositBin;
    public static int numDishesServed = 0;
    public TMP_Text showDishRemovedText;
    public Image dishRemovedIcon;
    public float displayDuration = 4f;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        showDishRemovedText.enabled = false;
        dishRemovedIcon.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject == depositBin) {
                    DepositDishesIntoBin();
                }
            }
        }
    }

    void DepositDishesIntoBin() {
        if (inventory != null && inventory.items.Count != 0) {
            // go through all of inventory items, and only remove if they are dishes
            for (int i=0; i<inventory.items.Count; i++) {
                if (inventory.items[i].isDish) {
                    Item currDish = inventory.items[i];

                    int currDishItemAmount = inventory.items[i].itemAmount;
                    numDishesServed += currDishItemAmount;
                    inventory.Remove(currDish); //removes all if stacked
                    ShowItemRemoved(currDish.name, currDish.icon);
                }
            }
        }
    }

    public void ShowItemRemoved(string itemName, Sprite icon) {
        showDishRemovedText.text = "Removed dish: " + itemName;
        dishRemovedIcon.sprite = icon;

        StartCoroutine(DisplayItemCoroutine());
    }

    private IEnumerator DisplayItemCoroutine() {
        showDishRemovedText.enabled = true;
        dishRemovedIcon.enabled = true;

        yield return new WaitForSeconds(displayDuration);

        showDishRemovedText.enabled = false;
        dishRemovedIcon.enabled = false;

    }
}
