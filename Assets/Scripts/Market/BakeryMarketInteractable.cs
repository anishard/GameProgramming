using UnityEngine;

public class BakeryMarketInteractable : MonoBehaviour
{
    public BakeryMarket bakeryMarket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the Player has the "Player" tag
        {
            bakeryMarket.ToggleMarketUI();
        }
    }
}