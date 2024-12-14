using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KitchenGame : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI todoText;  

    private float time = 60.0f;     // Total game time in seconds
    private List<string> todos;     // List of todos
    private int curTodoIndex;       // Index to track the current todo
    private bool run;               

    void Start()
    {
        todos = new List<string> {
            "Wash your hands for food safety!",
            "The kitchen is a mess, press [F] to clean up spills!", // TODO: middle section is randomized: spiders or cleanup
            "Spiders are attacking the kitchen, squash them!", 
            "Cook your food at the fireplace!"
        };

        curTodoIndex = 0;
        run = true;

        UpdateTodoText();
    }

    void Update()
    {
        if (run)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                EndGame();
            }

            UpdateTimerText();

            // TODO: remove this, use callbacks
            if (Input.GetKeyDown(KeyCode.C)) 
            {
                CompleteTodo();
            }
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Timer: " + time.ToString("F1");
    }

    void UpdateTodoText()
    {
        if (curTodoIndex < todos.Count)
        {
            todoText.text = "Todo: " + todos[curTodoIndex];
        }
        else
        {
            todoText.text = "All tasks complete!";
            EndGame();
        }
    }

    void CompleteTodo()
    {
        curTodoIndex++; 
        UpdateTodoText();
    }

    void EndGame()
    {
        time = 0;
        run = false;
        todoText.text = "Game Over!";
    }

    /*
     * Below are callbacks triggered when interacting with object of interest
     */ 

    public void WashHands() {
        Debug.Log("You washed your hands!");
    }
}