using UnityEngine;

public class MarketInteractable : MonoBehaviour
{
    public VegetableMarket vegetableMarket;

    // Trigger interaction when the player collides with the shop
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            vegetableMarket.ToggleMarketUI();
        }
    }
}