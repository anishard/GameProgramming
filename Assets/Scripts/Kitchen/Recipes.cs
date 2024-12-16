using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public TextMeshProUGUI hint; 
    public Canvas recipeUI;

    private Inventory inventory;

    void Start() 
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();

        Item meat = (Item)ScriptableObject.CreateInstance("Item");
        meat.name = "Meat";
        inventory.Add(meat);
        inventory.Add(meat);
    }

    void ShowHint() 
    {
        hint.text = "You don't have the required ingredients to cook that!";
        hint.gameObject.SetActive(true); 
        Invoke(nameof(HideHint), 1f); // Hint lingers for a second
    }

    void HideHint()
    {
        hint.gameObject.SetActive(false); 
    }

    void TakeItems(List<Item> items) {
        // Removes the specified items from the player inventory
        foreach (Item item in items) {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].name == item.name)
                {
                    inventory.items[i].itemAmount--;
                    if (inventory.items[i].itemAmount == 0) {
                        inventory.items.Remove(item);
                    }
                    break;
                }
            }
        }
    }

    List<Item> FindItems(string[] names) 
    {
        // Find the require items in the player's inventory
        List<Item> foundItems = new List<Item>();
        foreach (string name in names) {
            bool found = false;
            foreach (Item item in inventory.items) {
                if (item.name == name) {
                    foundItems.Add(item);
                    found = true;
                    break;
                }
            }   
            if (!found) {
                // Player is missing a required item
                return null;
            }
        }
        return foundItems;
    }

    void StartRecipe(string recipe, string[] ingredients) {
        List<Item> items = FindItems(ingredients);
        if (items != null) {
            recipeUI.gameObject.SetActive(false);
            TakeItems(items);
            FindObjectOfType<KitchenGame>().Play(recipe);
        }
        else {
            ShowHint();
        }
    }

    // Functions below are attached as callbacks in the UI

    public void SteakRecipe() {
        StartRecipe("Steak", new string[] {"Meat"});
    }
}
