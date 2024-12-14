using System.Collections.Generic;
using UnityEngine;

public class SpillCleaning : MonoBehaviour
{
    public GameObject spillZone; 
    public GameObject spillPrefab; 
    public AudioClip cleanSound;
    public int spillCount = 3;

    private GameObject[] spills;      
    private readonly float minSpillDist = 3.5f; 

    // State changed by Spill.cs
    internal GameObject curSpill; 
    internal int cleanedSpills = 0;

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

        // Create a list to store the positions of the spills
        List<Vector3> spillPositions = new List<Vector3>();

        for (int i = 0; i < spillCount; i++)
        {
            Vector3 randomPosition;
            bool positionFound = false;

            // Try to find a valid position for the spill
            while (!positionFound)
            {
                randomPosition = new Vector3(
                    Random.Range(zoneMin.x, zoneMax.x),
                    spillZone.transform.position.y, 
                    Random.Range(zoneMin.z, zoneMax.z)
                );

                bool overlap = false;
                foreach (var pos in spillPositions)
                {
                    // Check if the random position overlaps with any existing spill
                    if (Vector3.Distance(pos, randomPosition) < minSpillDist)
                    {
                        overlap = true;
                        break;
                    }
                }

                // If there's no overlap, add the position and instantiate the spill
                if (!overlap)
                {
                    positionFound = true;
                    spillPositions.Add(randomPosition);
                    GameObject spill = Instantiate(spillPrefab, randomPosition, Quaternion.identity);
                    spills[i] = spill;
                }
            }
        }
    }

    private void CleanSpill(GameObject spill)
    {
        // Remove the spill
        gameObject.GetComponent<AudioSource>().PlayOneShot(cleanSound);
        Destroy(spill);
        curSpill = null;
        cleanedSpills++;
    }
}