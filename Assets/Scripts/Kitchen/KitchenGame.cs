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
            "Clean up the spilsl!"
        };

        curTodoIndex = 0;
        run = true;

        UpdateTodoText();
    }

    void Update()
    {
        if (run)
        {
            // Update the timer
            time -= Time.deltaTime;

            if (time <= 0)
            {
                time = 0;
                run = false;
                EndGame();
            }

            // Update the timer text
            UpdateTimerText();

            // Check if the current todo is completed
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
            run = false; 
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
        todoText.text = "Game Over!";
    }
}