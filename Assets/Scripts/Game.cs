using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public static AudioSource audioSource;

    private static GameObject ui;
    private static Note uiNote;
    private static AudioClip[] audioClips;
    private static GameObject[] dialogues;
    private static TextAsset[] notes;


    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        ui = GameObject.Find("UI");
        uiNote = GameObject.Find("Note").GetComponent<Note>();
    }

    void Update() { }

    public static void ActivateDialogue(string name)
    {
        if (ui != null) ToggleUI(false);
        Player.TogglePlayer(false);

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = FindInArray(dialogues, name);
        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }

    public static void ActivateNote(string name)
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
            Note.ToggleNote(data.title, data.body);
        else
            throw new Exception(name + " does not exist in Resources/Notes");
    }

    public static bool ClickDetected(bool allowUI = false)
    {
        bool detected = false;

        bool mouseClicked = Input.GetMouseButtonDown(0);
        bool buttonClicked = Input.GetKeyDown(KeyCode.J);
        bool uiClicked = EventSystem.current.IsPointerOverGameObject();

        if ((mouseClicked && (allowUI || !uiClicked)) || buttonClicked)
            detected = true;

        return detected;
    }

    public static IEnumerator PlayAudio(string clipName, float volumeScale = 1f, float delay = 0f)
    {
        audioClips ??= Resources.LoadAll<AudioClip>("AudioClips");
        AudioClip clip = Array.Find(audioClips, (e) => e.name == clipName);

        if (clip == null)
            throw new Exception(clipName + " does not exist in Resources/AudioClips");

        yield return new WaitForSeconds((float)delay);

        audioSource.PlayOneShot(clip, (float)volumeScale);
    }

    public static void RemoveNote()
    {
        Note.ToggleNote();
    }

    public static void ToggleUI(bool isEnabled)
    {
        if (ui) ui.GetComponent<Canvas>().enabled = isEnabled;
    }

    private static GameObject FindInArray(GameObject[] array, string name)
    {
        GameObject obj = Array.Find(array, (o) => o.name == name);

        if (obj == null)
            throw new Exception(name + " does not exist in Assets/Resources");

        return obj;
    }
}
