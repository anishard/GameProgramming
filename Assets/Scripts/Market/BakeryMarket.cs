using UnityEngine;
using TMPro;

public class BakeryMarket : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory
    public TextMeshProUGUI walletText; // Wallet display
    public TextMeshProUGUI selectedItemText; // Display for selected item
    public GameObject marketUI; // The Market Panel

    private Item selectedItem; // Currently selected item for buying/selling
    private int randomSellPrice; // Random sell price between 3-5

    private void Start()
    {
        UpdateWalletUI();
        ClearSelection();
    }

    // Called when an item button is clicked
    public void SelectItem(Item item)
    {
        selectedItem = item;
        randomSellPrice = GetRandomSellPrice();
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
        if (Wallet.walletBalance >= itemCost) // Shared wallet balance
        {
            Wallet.walletBalance -= itemCost;
            bool added = inventory.Add(selectedItem);
            if (!added)
            {
                Debug.Log("Inventory full!");
                Wallet.walletBalance += itemCost; // Refund coins
            }
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough coins to buy!");
        }
    }

    // Sell the selected item
    public void SellSelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.Log("No item selected for selling!");
            return;
        }

        foreach (Item inventoryItem in inventory.items)
        {
            if (inventoryItem.name == selectedItem.name)
            {
                Wallet.walletBalance += randomSellPrice;
                inventory.Remove(inventoryItem);
                Debug.Log($"Sold {inventoryItem.name} for {randomSellPrice} coins!");
                ClearSelection();
                UpdateWalletUI();
                return;
            }
        }
        Debug.Log("Item not found in inventory!");
    }

    // Get buying price of an item
    private int GetItemCost(Item item)
    {
        return item.name switch
        {
            "Sugar" => 5,
            "Flour" => 4,
            "Rice" => 3,
            _ => 0
        };
    }

    // Random sell price between 3 and 5
    private int GetRandomSellPrice()
    {
        return Random.Range(3, 6); // 3 to 5 inclusive
    }

    // Update wallet UI
    private void UpdateWalletUI()
    {
        walletText.text = $"Coins: {Wallet.walletBalance}";
    }

    // Clear selection
    private void ClearSelection()
    {
        selectedItem = null;
        selectedItemText.text = "Selected: None";
    }

    // Toggle the Bakery Market UI
    public void ToggleMarketUI()
    {
        marketUI.SetActive(!marketUI.activeSelf);
        if (!marketUI.activeSelf)
        {
            ClearSelection();
        }
    }
}
