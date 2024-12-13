using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomSpawner : MonoBehaviour
{
    public GameObject spawnedItem;
    public float spawnRadius = 18f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Vector3 randSpawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randSpawnPos, out hit, spawnRadius, NavMesh.AllAreas)) {
                Instantiate(spawnedItem, hit.position, Quaternion.identity);
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnedItem.transform.position, 5f);
    }
}
