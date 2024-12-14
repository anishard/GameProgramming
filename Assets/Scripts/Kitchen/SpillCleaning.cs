using UnityEngine;

public class SpillCleaning : MonoBehaviour
{
    public GameObject spillZone; 
    public GameObject spillPrefab; 
    public int spillCount = 3;

    private GameObject[] spills;           

    // State changed by Spill.cs
    internal GameObject curSpill; 

    void Start()
    {
        CreateSpills();
    }

    void Update()
    {
        if (curSpill != null && Input.GetKeyDown(KeyCode.F))
        {
            // Cleanup spill player that the player is standing in
            CleanSpill(curSpill);
        }
    }

    private void CreateSpills()
    {
        spills = new GameObject[spillCount];

        // Get the bounds of the spillZone
        Renderer zoneRenderer = spillZone.GetComponent<Renderer>();
        Vector3 zoneMin = zoneRenderer.bounds.min;
        Vector3 zoneMax = zoneRenderer.bounds.max;

        for (int i = 0; i < spillCount; i++)
        {
            // Generate random position within the zone
            Vector3 randomPosition = new Vector3(
                Random.Range(zoneMin.x, zoneMax.x),
                spillZone.transform.position.y, 
                Random.Range(zoneMin.z, zoneMax.z)
            );

            // Instantiate the spill prefab at the random position
            GameObject spill = Instantiate(spillPrefab, randomPosition, Quaternion.identity);
            spills[i] = spill; 
        }
    }

    private void CleanSpill(GameObject spill)
    {
        // Remove the spill
        Debug.Log("Spill cleaned up!");
        Destroy(spill);
        curSpill = null;
    }
}