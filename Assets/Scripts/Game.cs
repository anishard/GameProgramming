using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int day;
    public int hour;

    private Player player;
    private GameObject[] dialogues;
    private float startTime;
    private readonly int hourInRealMinutes = 1;

    void Start()
    {
        day = 0;
        hour = 0;
        startTime = Time.time;

        player = GameObject.Find("Player").GetComponent<Player>();
        dialogues = Resources.LoadAll<GameObject>("Dialogues");

        ActivateDialogue("GameIntro");
    }

    void Update()
    {
        if (Time.time - startTime >= 60 * hourInRealMinutes)
        {
            hour++;
            startTime = Time.time;
        }

        if (hour == 24)
        {
            day++;
            hour = 0;
        }
    }

    public void ActivateDialogue(string name)
    {
        GameObject.Find("Interface").GetComponent<Canvas>().enabled = false;

        GameObject dialogue = Array.Find(dialogues, (d) => d.name == name);

        if (dialogue == null)
            throw new Exception(name + " does not exist in the given array");

        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }
}
