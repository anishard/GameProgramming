using UnityEngine;

public class MeatMarketInteractable : MonoBehaviour
{
    public MeatMarket meatMarket;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}"); // Add Debug Log

        if (other.CompareTag("Player")) // Ensure the Player tag matches
        {
            Debug.Log("Player detected! Opening Meat Market.");
            meatMarket.ToggleMarketUI();
        }
    }
}
