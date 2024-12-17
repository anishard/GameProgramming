using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class FeedVillagers : MonoBehaviour
{
    Inventory inventory;


    public GameObject villager;
    public GameObject player;


    public Item dishToRemove;
    private GameObject dishObject; // to be instantiated and destroyed when "feed"


    public GameObject endPosition; //the table, where the villager will stop
    public GameObject startPosition; //the villagerSpawner, where the villager will spawn
    public GameObject dishPosition; //where the plate is on the table


    public GameObject villagerSpawner;
    public float spawnRadius = 2f;


    public GameObject door;
    public GameObject table;
   
    private NavMeshAgent playerAgent;
    private NavMeshAgent villagerAgent;


    public GameObject serveButton;
    public GameObject eatButton;
    public GameObject nextButton;


    public static int numDishesServed = 0; // static variable to use for # dishes stat in end scene
    private int numDishesSoFar = 0; //running count of dishes served


    public GameObject tableSetUp;
    public TMP_Text statUI;


    private List<Item> allDishItemsInInventory = new List<Item>();
    private List<Item> invDishNoDuplicates = new List<Item>();


    private Button btnServe;
    private Button btnEat;
    private Button btnNext;


    private bool waitingForFood = false;


    // Start is called before the first frame update
    void Start()
    {
        // currScene = SceneManager.GetActiveScene();
        // if (currScene.name == "DiningRoom") {
            playerAgent = player.GetComponent<NavMeshAgent>();
            inventory = Inventory.instance;
         
            btnServe = serveButton.GetComponent<Button>();
            btnEat = eatButton.GetComponent<Button>();
            btnNext = nextButton.GetComponent<Button>();


            // serveButton.SetActive(true);
            // eatButton.SetActive(true);
            // nextButton.SetActive(true);


            // btnServe.interactable = true;
            // btnEat.interactable = true;
            // btnNext.interactable = true;


            // villager.SetActive(true);
            villagerAgent = villager.GetComponent<NavMeshAgent>();
            villagerAgent.enabled = true;
            villagerAgent.SetDestination(endPosition.transform.position);


            for (int i=0; i<inventory.items.Count; i++) {
                if (inventory.items[i].isDish) {
                    for (int j=0; j<inventory.items[i].itemAmount; j++) {
                        allDishItemsInInventory.Add(inventory.items[i]);
                    }
                }
            }
            for (int i=0; i<inventory.items.Count; i++) {
                if (inventory.items[i].isDish) {
                    for (int j=0; j<inventory.items[i].itemAmount; j++) {
                        if (!invDishNoDuplicates.Contains(inventory.items[i])) {
                            invDishNoDuplicates.Add(inventory.items[i]);
                        }
                    }
                }
            }
            // Debug.Log("invdishnodups count: " + invDishNoDuplicates.Count);
            // Debug.Log("invdishnodups itemAmount: " + invDishNoDuplicates[0].itemAmount);
            numDishesServed += allDishItemsInInventory.Count;
    }
    // }


    // Update is called once per frame
    void Update()
    {
            if (villagerAgent != null && villagerAgent.isOnNavMesh) {
                if (!waitingForFood && !villagerAgent.pathPending && villagerAgent.remainingDistance <= villagerAgent.stoppingDistance) {
                    btnServe.interactable = true;
                    btnEat.interactable = false;
                    btnNext.interactable = false;
                }
            }
           
            // make sure the player stays fixed while giving out food
            // playerAgent.velocity = Vector3.zero;
            // playerAgent.updateRotation = false;
            //inventory = Inventory.instance;
            // if (DeliverDish.hasPlayedDeliverIntro) { && DeliverDish.startMovingToExit) {
            //     // for (int i=0; i<inventory.items.Count; i++) {
            //     //     // Feed();
            //     // }
            //     DeliverDish.hasPlayedDeliverIntro = false;
            //     // DeliverDish.startMovingToExit = false;
            // }
           
            // if not done OR done, keep displaying text field
            if (numDishesSoFar <= allDishItemsInInventory.Count) {
                statUI.text = "Number of Dishes: " + numDishesSoFar.ToString("n0");
                //if done, also disable buttons and get rid of table/chair
                if (numDishesSoFar == allDishItemsInInventory.Count && !waitingForFood) {
                    StopServing();
                }
            }
    }
   
    public void StopServing() {
        //disable all UI buttons
        // serveButton.SetActive(false);
        // eatButton.SetActive(false);
        // nextButton.SetActive(false);
        // //startButton.enable = false;


        // tableSetUp.SetActive(false);
        // statUI.enabled = false;
        gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }


        villager.SetActive(false);


        playerAgent.isStopped = false; //re start the character's movements
        playerAgent.updateRotation = true; //allow for rotation to occur
        // player.SetActive(true);
        //show stat for how many dishes served so far
        // Dialogue.Activate("DeliverDishOutro");
    }


    GameObject SpawnVillager()
    {
            GameObject spawnedVillager = Instantiate(villager, startPosition.transform.position, Quaternion.identity);
            villager.SetActive(true);
            villagerAgent = spawnedVillager.GetComponent<NavMeshAgent>();
            villagerAgent.enabled = true;
            return spawnedVillager;
    }


    public void ClickServeButton() {
        waitingForFood = true;


        btnServe.interactable = false;
        btnEat.interactable = true;
        btnNext.interactable = false;


        //Debug.Log("clicked the serve button");
        if (invDishNoDuplicates.Count > 0) {
           
            dishToRemove = invDishNoDuplicates[0];
            string dishName = dishToRemove.name;
            //Debug.Log("dish name is: " + dishName);


            numDishesSoFar += dishToRemove.itemAmount; //keep adding to running count of dishes
            if (dishToRemove != null) {
                //Debug.Log(dishToRemove);
                inventory.Remove(dishToRemove); //removes all of a stacked item at once
                invDishNoDuplicates.Remove(dishToRemove); //also remove from list of no duplicate dish items
                //allDishItemsInInventory.Remove(dishToRemove); // remove the FIRST occurrence of dishToRemove
            }


            string dishPrefabName = dishName.Split('_')[0];
            // Debug.Log("dish prefab name is: " + dishPrefabName);


            GameObject dishPrefab = Resources.Load<GameObject>("InventorySprites/" + dishPrefabName);
            // get the prefab for the item and instantiate it on the table
            // Debug.Log("dish prefab is: " + dishPrefab);
            if (dishPrefab != null) {
                // Debug.Log("instantiated prefab into scene");
                dishObject = Instantiate(dishPrefab, dishPosition.transform.position, Quaternion.identity);
            }
           
        }
    }


    public void ClickEatButton() {
        //feedButton.interactable = false;
        waitingForFood = true;


        btnServe.interactable = false;
        btnEat.interactable = false;
        btnNext.interactable = true;
        // TODO: PLAY EATING SOUND HERE
        // Debug.Log("clicked eat button");
        if (dishObject != null) {
            // Debug.Log("ate the dish");
            Destroy(dishObject);
        }
    }


    // first villager disappears, another spawns
    public void ClickNextButton() {
        waitingForFood = false;


        // Debug.Log("clicked the next button");
        Destroy(villager); // destroy the previous villager
        GameObject spawnedVillager = SpawnVillager(); // spawn the next one
        villager = spawnedVillager; // set the old one to the new one


        // make it come up to the table
        if (villager != null)
        {
            // Debug.Log("next button clicked, making it walk to table");
            villager.SetActive(true);
            villagerAgent = villager.GetComponent<NavMeshAgent>();
            Animator villagerAnimator = villager.GetComponent<Animator>();


            villagerAnimator.enabled = true;
            villagerAgent.enabled = true;


            if (!villagerAgent.pathPending) {
                //Debug.Log("NavMeshAgent path is pending...");
                villagerAgent.SetDestination(endPosition.transform.position);
            }
        }
    }
}
