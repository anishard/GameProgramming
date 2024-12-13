using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public AudioSource audioSource;

    private GameObject ui;
    private Note uiNote;
    private AudioClip[] audioClips;
    private GameObject[] dialogues;
    private TextAsset[] notes;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        ui = GameObject.Find("UI");
        uiNote = GameObject.Find("Note").GetComponent<Note>();
    }

    void Update() {}

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

        NoteData data = null;

        foreach (var file in notes)
        {
            var json = JsonUtility.FromJson<NoteData>(file.text);
            if (json.objectName == name)
            {
                data = json;
                break;
            }
        }

        if (data != null)
            uiNote.ToggleNote(data.title, data.body);
        else
            throw new Exception(name + " does not exist in Resources/Notes");
    }

    public bool ClickDetected(bool allowUI = false)
    {
        bool detected = false;

        bool mouseClicked = Input.GetMouseButtonDown(0);
        bool buttonClicked = Input.GetKeyDown(KeyCode.J);
        bool uiClicked = EventSystem.current.IsPointerOverGameObject();

        if ((mouseClicked && (allowUI || !uiClicked)) || buttonClicked)
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
