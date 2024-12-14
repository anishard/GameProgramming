using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LostFoundGame : MonoBehaviour
{
    public bool hasPlayedIntroDialogue;

    public GameObject spawnedItem;
    public float spawnRadius = 18f;
    public GameObject forkSpawner;
    public GameObject monkey;

    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayedIntroDialogue = false;
        inventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: if the fork is put in the inventory, call StopLostFoundGame()
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            StartLostFoundGame();
        }
    }

    public void StartLostFoundGame() {
        if (!hasPlayedIntroDialogue) {
            // play the opening dialogue, and make sure the dialogue doesn't replay after it ends
            Dialogue.Activate("LostFoundIntro");
            hasPlayedIntroDialogue = true;

            // spawn the fork at a random place on the farm
            SpawnFork();

            // have the monkey start chasing the player (if player goes within certain radius of monkey)
            FollowPlayer fpScript = monkey.GetComponent<FollowPlayer>();
            if (fpScript != null) { // && fork hasn't been found
                fpScript.ChasePlayer();
            }
            else {
                Debug.Log("the script is not found");
            }

            // if player was caught
            // if (fpScript.hasBeenCaught) {
            //     Debug.Log("YOU HAVE BEEN CAUGHT BY MONKEY");
            //     fpScript.StopChasePlayer();
            // }
        }
    }

    public void StopLostFoundGame() {
        //stop the game
        Debug.Log("You found the muskrat's pitchfork! Good job!");

        //stop monkey from chasing you
        FollowPlayer fpScript = monkey.GetComponent<FollowPlayer>();
        if (fpScript != null) { // && fork hasn't been found
            fpScript.StopChasePlayer();
        }
        else {
            Debug.Log("the script is not found");
        }

        Dialogue.Activate("LostFoundOutro");

        // TODO: Collect the coins reward



    }

    private void SpawnFork() {
        Vector3 randSpawnPos = forkSpawner.transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randSpawnPos, out hit, spawnRadius, NavMesh.AllAreas)) {
            Instantiate(spawnedItem, hit.position, Quaternion.identity);
        }
    }

    public void StealFromInventory() {
        //if monkey collides with player, take something out of inventory
        int randInt = Random.Range(0, inventory.items.Count);

        if (inventory.items.Count == 0) {
            Debug.Log("You don't have any items in your inventory for the monkey to steal...He'll still chase you though!");
        }
        else {
            Debug.Log("Oh no! That monkey stole a " + inventory.items[randInt].name + " from you!");

            inventory.Remove(inventory.items[randInt]);
        }
        
        //TODO: activate UI with muskrat saying "Oh no! The monkey is taking your stuff!


    }

}
