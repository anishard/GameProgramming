using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LostFoundGame : MonoBehaviour
{
    private bool hasPlayedIntroDialogue;

    public GameObject spawnedItem;
    public float spawnRadius = 18f;
    public GameObject forkSpawner;
    public GameObject monkey;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayedIntroDialogue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            StartLostFoundGame();
        }
    }

    void StartLostFoundGame() {
        if (!hasPlayedIntroDialogue) {
            // play the opening dialogue, and make sure the dialogue doesn't replay after it ends
            Dialogue.Activate("LostFoundIntro");
            hasPlayedIntroDialogue = true;

            // spawn the fork at a random place on the farm
            SpawnFork();

            // have the monkey start chasing the player (if player goes within certain radius of monkey)
            FollowPlayer fpScript = monkey.GetComponent<FollowPlayer>();
            if (fpScript != null) {
                fpScript.ChasePlayer();
            }
            else {
                Debug.Log("the script is not found");
            }
        }
    }

    private void SpawnFork() {
        Vector3 randSpawnPos = forkSpawner.transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randSpawnPos, out hit, spawnRadius, NavMesh.AllAreas)) {
            Instantiate(spawnedItem, hit.position, Quaternion.identity);
        }
    }
}
