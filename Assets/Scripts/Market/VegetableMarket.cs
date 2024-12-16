using UnityEngine;
using TMPro;

public class VegetableMarket : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory
    public TextMeshProUGUI walletText; // Wallet display
    public TextMeshProUGUI selectedVegetableText; // Display for selected vegetable
    public GameObject marketUI; // The Market Panel
    public static int walletBalance = 100; // Initial wallet balance
    private Item selectedItem; // Currently selected vegetable

    private void Start()
    {
        UpdateWalletUI();
        ClearSelection();
    }

    // Called when a vegetable button is clicked
    public void SelectVegetable(Item item)
    {
        selectedItem = item;
        selectedVegetableText.text = $"Selected: {item.name}";
    }

    // Buy the selected vegetable
    public void BuySelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.Log("No vegetable selected!");
            return;
        }

        int itemCost = GetItemCost(selectedItem);
        if (walletBalance >= itemCost)
        {
            walletBalance -= itemCost;
            bool added = inventory.Add(selectedItem); // Try adding to inventory
            if (!added)
            {
                Debug.Log("Inventory full!");
                walletBalance += itemCost; // Refund coins if inventory full
            }
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    // Sell the selected vegetable
    public void SellSelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.Log("No vegetable selected!");
            return;
        }

        foreach (Item inventoryItem in inventory.items)
        {
            if (inventoryItem.name == selectedItem.name)
            {
                int itemSellPrice = GetItemSellPrice(selectedItem);
                walletBalance += itemSellPrice;
                inventory.Remove(inventoryItem);
                UpdateWalletUI();
                return;
            }
        }
        Debug.Log("Item not found in inventory!");
    }

    // Get the cost of a vegetable for buying
    private int GetItemCost(Item item)
    {
        return item.name switch
        {
            "Carrot" => 10,
            "Turnip" => 15,
            "Pumpkin" => 20,
            "Eggplant" => 12,
            "Tomato" => 8,
            "Corn" => 5,
            _ => 0
        };
    }

    // Get the price of a vegetable for selling
    private int GetItemSellPrice(Item item)
    {
        return GetItemCost(item) / 2;
    }

    // Update the wallet display
    private void UpdateWalletUI()
    {
        walletText.text = $"Coins: {walletBalance}";
    }

    // Clear the selection
    private void ClearSelection()
    {
        selectedItem = null;
        selectedVegetableText.text = "Selected: None";
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