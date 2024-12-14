using UnityEngine;

public class Spill : MonoBehaviour
{
    private SpillCleaning spillCleaning;

    void Start()
    {
        spillCleaning = FindObjectOfType<SpillCleaning>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is standing in this spill
            spillCleaning.curSpill = gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player left this spill
            spillCleaning.curSpill = null;
        }
    }
}