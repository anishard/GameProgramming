using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public TextMeshProUGUI hint; 
    public Canvas recipeUI;

    private Inventory inventory;
    private int hintsQueued = 0;

    void Start() 
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    public void ShowRecipes() {
        recipeUI.gameObject.SetActive(true);
        GameObject.Find("Sparrow").GetComponent<Interact>().ToggleOff();
    }

    public void HideRecipes() {
        recipeUI.gameObject.SetActive(false);
        GameObject.Find("Sparrow").GetComponent<Interact>().ToggleOn();
    }

    void ShowHint() 
    {
        hint.text = "You don't have the required ingredients to cook that!";
        hint.gameObject.SetActive(true); 
        hintsQueued++;
        Invoke(nameof(HideHint), 1f); // Hint lingers for a second
    }

    void HideHint()
    {
        hintsQueued--;
        if (hintsQueued == 0) {
            hint.gameObject.SetActive(false); 
        }
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
        StartRecipe("Steak", new string[] {"Meat", "Spices"});
    }

    public void SaladRecipe() {
        StartRecipe("Salad", new string[] {"Eggplant", "Carrot", "Tomato", "Turnip"});
    }

    public void WrapRecipe() {
        StartRecipe("Wrap", new string[] {"Fish", "Lettuce", "Flour"});
    }

    public void PizzaRecipe() {
        StartRecipe("Pizza", new string[] {"Cheese", "Tomato", "Flour"});
    }

    public void OmeleteRecipe() {
        StartRecipe("Omelete", new string[] {"Eggs", "Butter"});
    }

    public void CakeRecipe() {
        StartRecipe("Cake", new string[] {"Milk", "Eggs", "Sugar", "Carrot"});
    }

    public void PieRecipe() {
        StartRecipe("Pie", new string[] {"Sugar", "Pumpkin"});
    }
}
