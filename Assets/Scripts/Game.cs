using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public AudioSource audioSource;
    public int day;
    public int hour;

    private Player player;
    private EventSystem eventSys;
    private AudioClip[] audioClips;
    private GameObject[] dialogues;

    private float startTime;
    private readonly int gameHourInRealMinutes = 1;

    void Start()
    {
        day = 0;
        hour = 0;
        startTime = Time.time;

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        eventSys = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
    }

    void Update()
    {
        if (Time.time - startTime >= 60 * gameHourInRealMinutes)
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
        var ui = GameObject.Find("Interface")?.GetComponent<Canvas>();
        if (ui != null) ui.enabled = false;

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = Array.Find(dialogues, (d) => d.name == name);

        if (dialogue == null)
            throw new Exception(name + " does not exist in the given array");

        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }

    public bool ClickDetected()
    {
        bool detected = false;
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
        {
            bool uiClicked = eventSys && eventSys.IsPointerOverGameObject();
            if (!uiClicked) detected = true;
        }

        return detected;
    }

    public AudioClip GetAudioClip(string name)
    {
        audioClips ??= Resources.LoadAll<AudioClip>("AudioClips");
        return Array.Find(audioClips, (e) => e.name == name);
    }
}
