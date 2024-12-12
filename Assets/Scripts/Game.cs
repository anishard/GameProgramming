using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public AudioSource audioSource;
    public int day;
    public int hour;

    private Player player;
    private GameObject ui;
    private Note uiNote;
    private AudioClip[] audioClips;
    private GameObject[] dialogues;
    private TextAsset[] notes;

    private float startTime;
    private readonly int gameHourInRealMinutes = 1;

    void Start()
    {
        day = 0;
        hour = 0;
        startTime = Time.time;

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        ui = GameObject.Find("UI");
        uiNote = GameObject.Find("Note").GetComponent<Note>();
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
        if (ui != null) ToggleUI(false);

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = FindInArray(dialogues, name);
        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }

    public void ActivateNote(string name)
    {
        notes ??= Resources.LoadAll<TextAsset>("Notes");

        List<string> lines = new();

        TextAsset note = Array.Find(notes, (file) =>
        {
            lines.Clear();
            lines.AddRange(file.text.Split(Environment.NewLine));
            return lines[0] == name;
        });

        if (note != null)
            uiNote.ToggleNote(lines[1], lines[2]);
        else
            throw new Exception(name + " does not exist in Resources/Notes");
    }

    public bool ClickDetected()
    {
        bool detected = false;

        bool mouseClicked = Input.GetMouseButtonDown(0);
        bool uiClicked = EventSystem.current.IsPointerOverGameObject();

        if ((mouseClicked && !uiClicked) || Input.GetKeyDown(KeyCode.J))
            detected = true;

        return detected;
    }

    public AudioClip GetAudioClip(string name)
    {
        audioClips ??= Resources.LoadAll<AudioClip>("AudioClips");
        return Array.Find(audioClips, (e) => e.name == name);
    }

    public void RemoveNote()
    {
        if (uiNote != null) uiNote.ToggleNote();
    }

    public IEnumerator PlayAudio(string clipName, float volumeScale = 1f, float delay = 0f)
    {
        AudioClip clip = GetAudioClip(clipName);

        if (clip == null)
            throw new Exception(clipName + " does not exist in Resources/AudioClips");

        yield return new WaitForSeconds((float)delay);

        audioSource.PlayOneShot(clip, (float)volumeScale);
    }

    public void ToggleUI(bool isEnabled)
    {
        if (ui) ui.GetComponent<Canvas>().enabled = isEnabled;
    }

    private GameObject FindInArray(GameObject[] array, string name)
    {
        GameObject obj = Array.Find(array, (o) => o.name == name);

        if (obj == null)
            throw new Exception(name + " does not exist in Assets/Resources");

        return obj;
    }
}
