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
                // Freeze timer at 0 if started meter game, otherwise end
                time = 0;
                if (!meterGameUI.gameObject.activeSelf) {
                    todoText.text = "You ran out of time!";
                    Reset();
                } 
            }
        }
    }

    public void Play(string recipe) {
        kitchenGameUI.gameObject.SetActive(true);
        playing = true;
        curRecipe = recipe;

        // Begin todos
        curTodoIndex = -1;
        StartNextTodo();     
    }

    public void Reset() {
        gameObject.GetComponent<SpillCleaning>().Stop();
        gameObject.GetComponent<CookingMeter>().Stop();
        GameObject.Find("Sparrow").GetComponent<Interact>().ToggleOn();
        GameObject.Find("Bucket").GetComponent<Interact>().ToggleOff();
        firePlace.GetComponent<Interact>().ToggleOff();
        time = 30.0f;
        playing = false;
    }

    public void StartNextTodo() {
        curTodoIndex++;
        if (curTodoIndex >= todos.Count) {
            // Finished all tasks
            Debug.Log("Finished all todos");
            Reset();
        }
        else {
            // Update scene based on new todo
            switch (todos[curTodoIndex]) 
            {
                case Todo.WASH:
                    // Make water bucket interactable
                    bucket.GetComponent<Interact>().ToggleOn();
                    break;

                case Todo.CLEAN:
                    // Generate spills to be cleaned 
                    SpillCleaning spillCleaning = GetComponent<SpillCleaning>();
                    spillCleaning.Play();
                    break;

                case Todo.COOK:
                    // Make fireplace interactable
                    firePlace.GetComponent<Interact>().ToggleOn();
                    break;
            }
        }

        UpdateTodoText();
    }   

    void UpdateTodoText()
    {
        // Change text to todo's description
        if (curTodoIndex >= todos.Count) {
            todoText.text = "You made a " + cookQuality + " " + curRecipe + "!";
        }
        else {
            todoText.text = "Todo: " + todoDescs[(int)todos[curTodoIndex]];
        }
    }

    public void WashHands() {
        // Make bucket non-interactable and start next todo
        gameObject.GetComponent<AudioSource>().PlayOneShot(washHandsSound);
        GameObject.Find("Bucket").GetComponent<Interact>().ToggleOff();
        StartNextTodo();
    }

    public void CookAtFirePlace() {
        // Make fireplace non-interactable and show meter game
        firePlace.GetComponent<Interact>().ToggleOff();
        meterGameUI.gameObject.SetActive(true);
        gameObject.GetComponent<CookingMeter>().Play();
    }

    public void Sleep() {
        if (sleeping) {
            // Hide sleeping model
            GameObject.Find("Bed").GetComponent<Interact>().SetText("Press [F] to sleep");
            sleepingPlayer.SetActive(false);
            player.SetActive(true);
            sleeping = false;
        }
        else {
            // Show sleeping player model
            GameObject.Find("Bed").GetComponent<Interact>().SetText("Press [F] to stop sleeping");
            player.SetActive(false);
            sleepingPlayer.SetActive(true);
            sleeping = true;
        }
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
}