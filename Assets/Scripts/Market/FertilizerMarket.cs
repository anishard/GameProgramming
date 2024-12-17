using UnityEngine;
using TMPro;

public class FertilizerMarket : MonoBehaviour
{
    public Inventory inventory; // Reference to the Inventory
    public TextMeshProUGUI walletText; // Wallet display
    public TextMeshProUGUI selectedItemText; // Display for selected seed/fertilizer
    public GameObject marketUI; // The Market Panel
    private Item selectedItem; // Currently selected seed or fertilizer

    private void Start()
    {
        UpdateWalletUI();
        ClearSelection();
    }

    // Called when a seed or fertilizer button is clicked
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
            Debug.Log("No item selected!");
            return;
        }

        int itemCost = GetItemCost(selectedItem);
        if (Wallet.walletBalance >= itemCost) // Access balance from VegetableMarket
        {
            Wallet.walletBalance -= itemCost;
            bool added = inventory.Add(selectedItem); // Add to inventory
            if (!added)
            {
                Debug.Log("Inventory full!");
                Wallet.walletBalance += itemCost; // Refund coins if inventory full
            }
            UpdateWalletUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    // Sell the selected item
    public void SellSelectedItem()
    {
        if (selectedItem == null)
        {
            Debug.Log("No item selected!");
            return;
        }

        foreach (Item inventoryItem in inventory.items)
        {
            if (inventoryItem.name == selectedItem.name)
            {
                int itemSellPrice = GetItemSellPrice(selectedItem);
                Wallet.walletBalance += itemSellPrice; // Add coins to balance
                inventory.Remove(inventoryItem);
                UpdateWalletUI();
                return;
            }
        }
        Debug.Log("Item not found in inventory!");
    }

    // Get the cost for buying items
    private int GetItemCost(Item item)
    {
        return item.name switch
        {
            "CarrotSeed" => 2,
            "TurnipSeed" => 2,
            "PumpkinSeed" => 2,
            "EggplantSeed" => 1,
            "TomatoSeed" => 1,
            "CornSeed" => 1,
            "Fertilizer" => 2,
            _ => 0
        };
    }

    // Get the price for selling items
    private int GetItemSellPrice(Item item)
    {
        return GetItemCost(item) / 2;
    }

    // Update the wallet UI
    private void UpdateWalletUI()
    {
        walletText.text = $"Coins: {Wallet.walletBalance}";
    }

    // Clear the selection
    private void ClearSelection()
    {
        selectedItem = null;
        selectedItemText.text = "Selected: None";
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