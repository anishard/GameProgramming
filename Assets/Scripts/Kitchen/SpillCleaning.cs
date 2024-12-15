using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpillCleaning : MonoBehaviour
{
    public GameObject spillZone; 
    public GameObject spillPrefab; 
    public AudioClip cleanSound;
    public int spillCount = 3;

    private KitchenGame kitchenGame;
    private GameObject[] spills; 
    private readonly float minSpillDist = 3f;    

    // State changed by Spill.cs
    internal GameObject curSpill; 
    internal int cleanedSpills = 0;

    void Start()
    {
        kitchenGame = FindObjectOfType<KitchenGame>();
        spills = new GameObject[spillCount];
        CreateSpills();
    }

    void Update()
    {
        if (cleanedSpills == spillCount) {
            // Todo complete, start next
            kitchenGame.StartNextTodo();
            this.enabled = false;
            return;
        }

        if (curSpill != null && Input.GetKeyDown(KeyCode.F))
        {
            // Cleanup spill that the player is standing in
            CleanSpill(curSpill);
        }
    }

    private void CreateSpills()
    {
        // Bounds of the spill zone
        Renderer zoneRenderer = spillZone.GetComponent<Renderer>();
        Vector3 zoneMin = zoneRenderer.bounds.min;
        Vector3 zoneMax = zoneRenderer.bounds.max;

        List<Vector3> spillPositions = new List<Vector3>();
        for (int i = 0; i < spillCount; i++)
        {
            bool positionFound = false;
            int attempts = 0;
            Vector3 randomPosition;

            while (!positionFound && attempts < 50)
            {
                // Generate a random position within the spill zone
                randomPosition = new Vector3(
                    Random.Range(zoneMin.x, zoneMax.x),
                    spillZone.transform.position.y, 
                    Random.Range(zoneMin.z, zoneMax.z)
                );

                // Check for overlap with existing spills
                bool overlap = false;
                foreach (var pos in spillPositions)
                {
                    // Check if other spills are too close
                    if (Vector3.Distance(pos, randomPosition) < minSpillDist) 
                    {
                        overlap = true;
                        break;
                    }
                }

                // If there's no overlap, place the spill
                if (!overlap)
                {
                    positionFound = true;
                    spillPositions.Add(randomPosition);
                    GameObject spill = Instantiate(spillPrefab, randomPosition, Quaternion.identity);
                    spills[i] = spill;
                }
                attempts++;
            }

            // If a valid position couldn't be found after 50 attempts, log a warning
            if (!positionFound)
            {
                Debug.LogWarning($"Unable to place spill {i + 1} after 50 attempts.");
                cleanedSpills++;
            }
        }
    }

    private void CleanSpill(GameObject spill)
    {
        // Remove the spill
        gameObject.GetComponent<AudioSource>().PlayOneShot(cleanSound, 0.1f);
        Destroy(spill);
        curSpill = null;
        cleanedSpills++;
    }
}