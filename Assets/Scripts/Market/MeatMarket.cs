using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MeatMarket : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory
    public TextMeshProUGUI walletText; // Wallet display
    public TextMeshProUGUI selectedItemText; // Display for selected item (Eggs, Fish, etc.)
    public TextMeshProUGUI sellPriceText; // Random price display for dishes
    public GameObject marketUI; // The Market Panel

    private Item selectedItem; // Currently selected item for buying
    private Item selectedDish; // Currently selected dish for selling
    private int currentSellPrice; // Randomly generated price for selling dishes

    private void Start()
    {
        UpdateWalletUI();
        ClearSelection();
    }

    // Select item for buying
    public void SelectItem(Item item)
    {
        selectedItem = item;
        selectedItemText.text = $"Selected: {item.name}";
    }

    // Buy the selected item
    public void BuySelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.Log("No item selected for buying!");
            return;
        }

        int itemCost = GetItemCost(selectedItem);
        if (VegetableMarket.walletBalance >= itemCost) // Shared wallet balance
        {
            VegetableMarket.walletBalance -= itemCost;
            inventory.Add(selectedItem);
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy!");
        }
    }

    // Select a dish to generate a random sell price
    public void SelectDishToSell(Item dish)
    {
        selectedDish = dish;

        if (inventory.items.Contains(dish))
        {
            currentSellPrice = Random.Range(10, 50); // Generate random price between 10-50 coins
            sellPriceText.text = $"Sell {dish.name} for {currentSellPrice} coins?";
        }
        else
        {
            Debug.Log("Dish not found in inventory!");
            ClearSelection();
        }
    }

    // Confirm selling the dish
    public void SellSelectedDish()
    {
        if (selectedDish == null || !inventory.items.Contains(selectedDish))
        {
            Debug.Log("No valid dish selected!");
            return;
        }

        // Remove dish from inventory and add coins
        VegetableMarket.walletBalance += currentSellPrice;
        inventory.Remove(selectedDish);
        Debug.Log($"Sold {selectedDish.name} for {currentSellPrice} coins!");

        ClearSelection();
        UpdateWalletUI();
    }

    // Get the cost for buying items (meat only)
    private int GetItemCost(Item item)
    {
        return item.name switch
        {
            "Eggs" => 3,
            "Fish" => 5,
            "Meat" => 15,
            "Chicken" => 10,
            _ => 0
        };
    }

    // Update the wallet UI
    private void UpdateWalletUI()
    {
        walletText.text = $"Coins: {VegetableMarket.walletBalance}";
    }

    // Clear selected items
    private void ClearSelection()
    {
        selectedItem = null;
        selectedDish = null;
        selectedItemText.text = "Selected: None";
        sellPriceText.text = "";
    }

    // Toggle the market UI
    public void ToggleMarketUI()
    {
        marketUI.SetActive(!marketUI.activeSelf);
        if (!marketUI.activeSelf)
        {
            ClearSelection();
        }
    }
}
