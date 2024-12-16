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
    public Item plate;
    private int numBrokenPlates;
    public GameObject objectToSceneTransition;

    Inventory inventory;
    FollowPlayer fpScript; 

    private NPC npc;
    private bool enableStart;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayedIntroDialogue = false;
        brokePlate = false;
        gameIsOver = false;
        numBrokenPlates = 0;
        enableStart = false;
        inventory = Inventory.instance;
        npc = GetComponent<NPC>();
        fpScript = monkey.GetComponent<FollowPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Clock.day > 0 && npc.CanOfferQuest && !gameIsOver)
        {
            enableStart = true;
            npc.ActivateAlert();
        }

        // if Player clicks on Pudu NPC
        if (enableStart && !gameIsOver && npc.ClickDetected())
        {
            npc.RemoveAlert();
            npc.isMidQuest = true;

            PreventSceneChange pscScript = objectToSceneTransition.GetComponent<PreventSceneChange>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // if first time talking to NPC, play intro quest dialogue 
                if (!hasPlayedIntroDialogue) {
                    pscScript.preventTriggerSceneChange();
                    StartCollectPlateGame();
                    hasPlayedIntroDialogue = true;
                }
                
                // if player has collected all plates in scene (including broken ones), remove the plates from inventory and stop game
                // Debug.Log("there are: " + numBrokenPlates.ToString("n0") + " broken plates.");
                // Debug.Log("there are: " + numPlatesInInventory().ToString("n0") + " plates in inventory.");
                int numPlatesCollected = numPlatesInInventory() + numBrokenPlates;
                if (numPlatesCollected == numPlatesToCollect) {
                    Item plate = null;
                    foreach (Item item in inventory.items) {
                        if (item.name == "Plate") {
                            plate = item; // Store the item if it matches
                            break; // Exit the loop once the item is found
                        }
                    }
                    //remove the plates from inventory when ALL are collected
                    if (plate != null && hasPlayedIntroDialogue) {
                        pscScript.enableTriggerSceneChange();
                        StopCollectPlateGame(); //which will play ending dialogue

                        // remove all the plates from the inventory
                        int numPlates = numPlatesInInventory();
                        for (int i=0; i<numPlates; i++) {
                            inventory.Remove(plate);
                        }     
                    }
                }
                if (numPlatesCollected < numPlatesToCollect && !gameIsOver) {
                    int platesLeft = numPlatesToCollect - numPlatesCollected;
                    //Debug.Log("There are still " + platesLeft.ToString("n0") + " plates left.");
                    Tip.Activate("There are still some plates left. Please go get them for me, thanks!", 5);
                }
            }
        }

         if (Input.GetMouseButtonDown(0) && brokePlate) {
            brokePlate = false;
            Tip.Remove();
        }     
    }


    public void StartCollectPlateGame() {
        // play the opening dialogue, and make sure the dialogue doesn't replay after it ends
        monkey.SetActive(true);
        Dialogue.Activate("CollectPlateIntro");
        hasPlayedIntroDialogue = true;

        // spawn the plates at a random places in dining hall
        for (int i=0; i<numPlatesToCollect; i++) {
            SpawnPlate();
        }

        // have the monkey start chasing the player (if player goes within certain radius of monkey)
        if (fpScript == null) { 
            Debug.Log("the script is not found");
        }
    }

    public void StopCollectPlateGame() {
        monkey.SetActive(false);

        gameIsOver = true;
        npc.isMidQuest = false;
        if (numBrokenPlates == 0) {
            Dialogue.Activate("CollectPlateOutroNoBrokenPlates");
        }
        else {
            Dialogue.Activate("CollectPlateOutroBrokePlate");
        }
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
            //remove a plate from inventory
            inventory.Remove(plate);
            //plate tip that the plate has been broken
            Tip.Activate("Oh no! The monkey broke a plate! Hurry, collect the rest of them before he breaks any more!", 5);
            //keep count of how many broken plates there are (+ num plates left in inventory = total)
            numBrokenPlates += 1;

            if (Tip.isActive && !brokePlate) {
                brokePlate = true;
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
