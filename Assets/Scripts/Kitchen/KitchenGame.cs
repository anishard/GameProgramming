using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum Todo
{
    WASH = 0,
    CLEAN = 1,
    COOK = 2
}

public class KitchenGame : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI todoText;  
    public AudioClip washHandsSound;
    public AudioClip crushSound;
    public Canvas meterGameUI;
    public GameObject spiderPrefab;
    public GameObject spawnZone;
    public GameObject sleepingPlayer;
    public GameObject player;
    public Canvas kitchenGameUI;

    private float time = 30.0f;   
    private List<Todo> todos;     
    private int curTodoIndex;   
    private bool playing;    
    private GameObject bucket;
    private GameObject firePlace; 
    private bool sleeping = false;

    internal string cookQuality;
    internal string curRecipe;

    // Map todo ID to its description
    private readonly string[] todoDescs = new string[] {
        "Wash your hands for food safety!",
        "Whoops! You made a few spills. Press [F] on them to clean up!", 
        "Go to the fire place and cook your food!"
    };          

    void Start()
    {
        // Todos that the player will have to complete 
        todos = new List<Todo>
        {
            Todo.WASH,
            Todo.CLEAN,
            Todo.COOK
        };
        bucket = GameObject.Find("Bucket");
        firePlace = GameObject.Find("FirePlaceFull");
        SpawnSpiderRandomly();
    }

    void Update()
    {
        if (playing)
        {    
            // Update countdown timer
            time -= Time.deltaTime; 
            timerText.text = "Timer: " + time.ToString("F1");
            if (time <= 0)
            {
                time = 0;
                playing = false;
                todoText.text = "You ran out of time!";
            }
        }
    }

    public void Play(string recipe) {
        Debug.Log("hit play!");
        kitchenGameUI.gameObject.SetActive(true);
        playing = true;
        curRecipe = recipe;

        // Begin todos
        curTodoIndex = -1;
        StartNextTodo();     
    }

    public void StartNextTodo() {
        curTodoIndex++;
        if (curTodoIndex >= todos.Count) {
            // Finished all tasks
            Debug.Log("Finished all todos");
            playing = false;
        }
        else {
            // Update scene based on new todo
            switch (todos[curTodoIndex]) 
            {
                case Todo.WASH:
                    // Make water bucket interactable
                    bucket.GetComponent<Interact>().enabled = true;
                    break;

                case Todo.CLEAN:
                    // Generate spills to be cleaned
                    SpillCleaning spillCleaning = GetComponent<SpillCleaning>();
                    spillCleaning.enabled = true;
                    break;

                case Todo.COOK:
                    // Make fireplace interactable
                    firePlace.GetComponent<Interact>().enabled = true;
                    break;
            }
        }

        UpdateTodoText();
    }   

    void UpdateTodoText()
    {
        // Change text to todo's description
        if (curTodoIndex >= todos.Count) {
            todoText.text = "You made a " + cookQuality.ToLower() + " dish!";
        }
        else {
            todoText.text = "Todo: " + todoDescs[(int)todos[curTodoIndex]];
        }
    }

    public void WashHands() {
        gameObject.GetComponent<AudioSource>().PlayOneShot(washHandsSound);

        // Make bucket non-interactable
        GameObject bucket = GameObject.Find("Bucket");
        bucket.GetComponent<Interact>().Toggle();

        // Task complete, start next
        StartNextTodo();
    }

    public void CookAtFirePlace() {
        Debug.Log("Cooking!");

        // Make fireplace non-interactable
        firePlace.GetComponent<Interact>().Toggle();

        meterGameUI.gameObject.SetActive(true);
    }

    void SpawnSpiderRandomly()
    {
        float spawnChance = Random.Range(0f, 1f);  
        float chanceThreshold = 0.5f;       
        if (spawnChance < chanceThreshold)
        {
            // Get the bounds of the spawnZone (assuming it's a plane)
            Renderer zoneRenderer = spawnZone.GetComponent<Renderer>();
            Vector3 zoneMin = zoneRenderer.bounds.min;
            Vector3 zoneMax = zoneRenderer.bounds.max;

            // Generate a random position within the spawnZone bounds
            Vector3 randomPosition = new Vector3(
                Random.Range(zoneMin.x, zoneMax.x),
                spawnZone.transform.position.y,  // Place the spider on the same Y as the spawnZone
                Random.Range(zoneMin.z, zoneMax.z)
            );

            // Instantiate the spider prefab at the random position
            Instantiate(spiderPrefab, randomPosition, Quaternion.identity);
        }
    }

    public void Sleep() {
        if (sleeping) {
            GameObject.Find("Bed").GetComponent<Interact>().SetText("Press [F] to sleep");
            sleepingPlayer.SetActive(false);
            player.SetActive(true);
            sleeping = false;
        }
        else {
            GameObject.Find("Bed").GetComponent<Interact>().SetText("Press [F] to stop sleeping");
            player.SetActive(false);
            sleepingPlayer.SetActive(true);
            sleeping = true;
        }
    }
}