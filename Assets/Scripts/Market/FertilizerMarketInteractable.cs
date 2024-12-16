using UnityEngine;

public class FertilizerMarketInteractable : MonoBehaviour
{
    public FertilizerMarket fertilizerMarket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            fertilizerMarket.ToggleMarketUI();
        }
    }
}
