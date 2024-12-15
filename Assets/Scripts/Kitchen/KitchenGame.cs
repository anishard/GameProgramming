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
    public Canvas meterGameUI;
    public GameObject spiderPrefab;
    public GameObject spawnZone;

    private float time = 45.0f;   
    private List<Todo> todos;     
    private int curTodoIndex;   
    private bool playing;    
    private GameObject bucket;
    private GameObject firePlace; 

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
        curTodoIndex = -1; // Will get updated to 0 by the StartNextTodo
        playing = true;
        bucket = GameObject.Find("Bucket");
        firePlace = GameObject.Find("FirePlaceFull");
        SpawnSpiderRandomly();
        StartNextTodo(); // TODO: move this trigger to the NPC
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
                EndGame();
            }
        }
    }

    public void StartNextTodo() {
        curTodoIndex++;
        if (curTodoIndex >= todos.Count) {
            // Finished all tasks
            Debug.Log("Finished all todos");
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
            todoText.text = "";
        }
        else {
            todoText.text = "Todo: " + todoDescs[(int)todos[curTodoIndex]];
        }
    }

    void EndGame()
    {
        time = 0;
        playing = false;
        todoText.text = "Game Over!";
    }

    public void WashHands() {
        gameObject.GetComponent<AudioSource>().PlayOneShot(washHandsSound);

        // Make bucket non-interactable
        GameObject bucket = GameObject.Find("Bucket");
        Interact interact = bucket.GetComponent<Interact>();
        interact.ClearText();
        interact.enabled = false;

        // Task complete, start next
        StartNextTodo();
    }

    public void CookAtFirePlace() {
        Debug.Log("Cooking!");

        // Make fireplace non-interactable
        Interact interact = firePlace.GetComponent<Interact>();
        interact.ClearText();
        interact.enabled = false;

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
}