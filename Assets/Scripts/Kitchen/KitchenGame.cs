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

    private float time = 60.0f;   
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
        curTodoIndex = 0;
        playing = true;
        bucket = GameObject.Find("Bucket");
        firePlace = GameObject.Find("FirePlaceFull");
        UpdateTodoText();
    }

    void Update()
    {
        if (playing)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                EndGame();
            }

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

                    // End task when player has cleaned all spills
                    if (spillCleaning.cleanedSpills == spillCleaning.spillCount) {
                        spillCleaning.enabled = false;
                        StartNextTodo();
                    }
                    break;

                case Todo.COOK:
                    // Make fireplace interactable
                    firePlace.GetComponent<Interact>().enabled = true;
                    break;
            }

            timerText.text = "Timer: " + time.ToString("F1");
        }
    }

    void StartNextTodo() {
        curTodoIndex++;
        UpdateTodoText();
    }   

    void UpdateTodoText()
    {
        // Change text to todo's description
        todoText.text = "Todo: " + todoDescs[(int)todos[curTodoIndex]];
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
    }
}