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
        Invoke(nameof(HideHint), 1f); 
    }

    void HideHint()
    {
        hint.gameObject.SetActive(false); 
    }

    void TakeItems(List<Item> items) {
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

    void DoRecipe(string recipe, string[] ingredients) {
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

    public void SteakRecipe() {
        DoRecipe("Steak", new string[] {"Meat"});
    }
}
