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
    private bool stolenFrom;
    public bool gameIsOver;
    public GameObject objectToSceneTransition; //set to whatever is the trigger for switching scenes

    Inventory inventory;
    FollowPlayer fpScript;

    private NPC npc;
    private bool enableStart;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayedIntroDialogue = false;
        stolenFrom = false;
        gameIsOver = false;
        enableStart = false;
        inventory = Inventory.instance;
        npc = GetComponent<NPC>();
        fpScript = monkey.GetComponent<FollowPlayer>();
        // objSceneTransitionColl = objectToSceneTransition.GetComponent<Collider>();
        // objSceneTransitionColl.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (npc.CanOfferQuest && !gameIsOver)
        {
            enableStart = true;
            npc.ActivateAlert();
        }

        if (enableStart && !gameIsOver && !Dialogue.isActive && npc.ClickDetected())
        {
            npc.RemoveAlert();
            npc.isMidQuest = true;

            PreventSceneChange pscScript = objectToSceneTransition.GetComponent<PreventSceneChange>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if click on muskrat
            if (Physics.Raycast(ray, out hit))
            {
                if (!hasPlayedIntroDialogue)
                {
                    pscScript.preventTriggerSceneChange();
                    //objSceneTransitionColl.isTrigger = false; // disable the trigger to switch scenes
                    //Debug.Log(objSceneTransitionColl.isTrigger);
                    StartLostFoundGame();
                    hasPlayedIntroDialogue = true;
                }
                // if the fork has been put in the inventory, and player goes up to muskrat again, end game
                // also, remove the fork from the inventory (to "return to the muskrat")
                Item fork = null;
                foreach (Item item in inventory.items)
                {
                    if (item.name == "Fork")
                    {
                        fork = item; // Store the item if it matches
                        break; // Exit the loop once the item is found
                    }
                }
                if (fork != null && hasPlayedIntroDialogue)
                {
                    pscScript.enableTriggerSceneChange();
                    //objSceneTransitionColl.isTrigger = true; // re-enable the trigger to switch scenes
                    StopLostFoundGame();
                    inventory.Remove(fork);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && stolenFrom)
        {
            stolenFrom = false;
            //Debug.Log("disable the UI again");
            Tip.Remove();
        }
    }

    public void StartLostFoundGame()
    {
        // play the opening dialogue, and make sure the dialogue doesn't replay after it ends
        monkey.SetActive(true);
        Dialogue.Activate("LostFoundIntro");
        hasPlayedIntroDialogue = true;

        // spawn the fork at a random place on the farm
        SpawnFork();

        // have the monkey start chasing the player (if player goes within certain radius of monkey)
        if (fpScript == null)
        {
            Debug.Log("the script is not found");
        }
    }

    public void StopLostFoundGame()
    {
        //stop monkey from chasing you
        monkey.SetActive(false);
        if (fpScript == null)
        {
            Debug.Log("the script is not found");
        }

        gameIsOver = true;
        npc.isMidQuest = false;
        Dialogue.Activate("LostFoundOutro");
        // TODO: Collect the coins reward


    }

    private void SpawnFork()
    {
        Vector3 randSpawnPos = forkSpawner.transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randSpawnPos, out hit, spawnRadius, NavMesh.AllAreas))
        {
            Instantiate(spawnedItem, hit.position, Quaternion.identity);
        }
    }

    public void StealFromInventory()
    {
        //FollowPlayer fpScript = monkey.GetComponent<FollowPlayer>();
        //if monkey collides with player, take something out of inventory
        int randInt = Random.Range(0, inventory.items.Count);

        if (inventory.items.Count == 0)
        {
            //Debug.Log("You don't have any items in your inventory for the monkey to steal...He'll still chase you though!");
        }
        //there is at least 1 item in inventory
        else
        {
            // get random item in inventory - if it's the fork, spawn another fork
            Item itemToRemove = inventory.items[randInt];
            inventory.Remove(itemToRemove);

            if (itemToRemove.name == "Fork")
            {
                //Debug.Log("removing the fork.");
                Tip.Activate("Oh no! He took the fork! It's somewhere on this farm though, please find it!", 3);

                if (Tip.isActive && !stolenFrom)
                {
                    stolenFrom = true;
                }
                SpawnFork(); // spawn the fork in another place if lost it
            }
            else
            {
                Tip.Activate("Oh no! That monkey might've stolen from you too! Better check your inventory…", 3);

                if (Tip.isActive && !stolenFrom)
                {
                    stolenFrom = true;
                }
            }
        }

    }

}
