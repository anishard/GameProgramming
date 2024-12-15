using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectPlateGame : MonoBehaviour
{
    public bool hasPlayedIntroDialogue;

    public GameObject spawnedItem;
    public float spawnRadius;
    public GameObject plateSpawner;
    public GameObject monkey;
    private bool brokePlate;
    public bool gameIsOver;
    private int numPlatesToCollect = 10;

    Inventory inventory;
    FollowPlayer fpScript; 

    // Start is called before the first frame update
    void Start()
    {
        hasPlayedIntroDialogue = false;
        brokePlate = false;
        gameIsOver = false;
        inventory = Inventory.instance;
        fpScript = monkey.GetComponent<FollowPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player FIRST goes up to muskrat, start playing the dialogue and start game
        //Debug.Log("clicked on muskrat");
        if (Game.ClickDetected() && Player.ObjectDetected("Pudu"))
        {
            //Debug.Log("clicked on pudu");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if click on muskrat
            if (Physics.Raycast(ray, out hit))
            {
                if (!hasPlayedIntroDialogue) {
                    StartCollectPlateGame();
                    hasPlayedIntroDialogue = true;
                }
                // if the fork has been put in the inventory, and player goes up to muskrat again, end game
                // also, remove the fork from the inventory (to "return to the muskrat")
                Item plate = null;
                foreach (Item item in inventory.items) {
                    if (item.name == "Plate") {
                        plate = item; // Store the item if it matches
                        break; // Exit the loop once the item is found
                    }
                }
                if (plate != null && hasPlayedIntroDialogue) {
                    StopCollectPlateGame();
                    // remove all the plates from the inventory
                    int numPlates = numPlatesInInventory();
                    for (int i=0; i<numPlates; i++) {
                        inventory.Remove(plate);
                    }     
                }
            }
        }

         if (Input.GetMouseButtonDown(0) && brokePlate) {
            brokePlate = false;
            
            Tip.Remove();
            if (fpScript != null) {
                fpScript.ChasePlayer();
            }
        }     
    }


    public void StartCollectPlateGame() {
        // play the opening dialogue, and make sure the dialogue doesn't replay after it ends
        Dialogue.Activate("CollectPlateIntro");
        hasPlayedIntroDialogue = true;

        // spawn the plates at a random places in dining hall
        for (int i=0; i<numPlatesToCollect; i++) {
            SpawnPlate();
        }

        // have the monkey start chasing the player (if player goes within certain radius of monkey)
        if (fpScript != null) { 
            fpScript.ChasePlayer();
        }
        else {
            Debug.Log("the script is not found");
        }
    }

    public void StopCollectPlateGame() {
        //stop monkey from chasing you
        if (fpScript != null) { 
            fpScript.StopChasePlayer();
        }
        else {
            Debug.Log("the script is not found");
        }

        gameIsOver = true;
        Dialogue.Activate("CollectPlateOutro");
        // TODO: Collect the coins reward


    }

    private void SpawnPlate() {
        Vector3 randSpawnPos = plateSpawner.transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randSpawnPos, out hit, spawnRadius, NavMesh.AllAreas)) {
            Instantiate(spawnedItem, hit.position, Quaternion.identity);
        }
    }

    public void BreakPlate() {

        if (inventory.items.Count == 0 || numPlatesInInventory() == 0) {
            //skip
        }
        //there is at least 1 PLATE in inventory
        else {
            //plate tip that the plate has been broken
            Tip.Activate("CollectPlateBreak", 5);
           
            if (Tip.isActive && !brokePlate) {
                brokePlate = true;
                if (fpScript != null) { 
                    fpScript.StopChasePlayer();
                }
            }
        }    
    }

    private int numPlatesInInventory() {
        foreach (Item item in inventory.items) {
            if (item.name == "Plate") {
                return item.itemAmount;
            }
        }
        return 0;
    }

}
