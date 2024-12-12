using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public AudioSource audioSource;
    public int day;
    public int hour;

    private Player player;
    private GameObject ui;
    private EventSystem eventSys;
    private AudioClip[] audioClips;
    private GameObject[] dialogues;
    private GameObject[] tips;

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
        if (ui != null) ToggleUI(false);

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = FindInArray(dialogues, name);
        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }

    public void ActivateTip(string name)
    {
        tips ??= Resources.LoadAll<GameObject>("Tips");

        GameObject tip = FindInArray(tips, name, true);

        if (tip != null)
        {
            GameObject obj = Instantiate(tip, Vector3.zero, Quaternion.identity, ui.transform);
            obj.transform.position = Tip.position;
        }
    }

    public bool ClickDetected()
    {
        bool detected = false;

        bool mouseClicked = Input.GetMouseButtonDown(0);
        bool uiClicked = eventSys && eventSys.IsPointerOverGameObject();

        if ((mouseClicked && !uiClicked) || Input.GetKeyDown(KeyCode.J))
            detected = true;

        return detected;
    }

    public AudioClip GetAudioClip(string name)
    {
        audioClips ??= Resources.LoadAll<AudioClip>("AudioClips");
        return Array.Find(audioClips, (e) => e.name == name);
    }

    public void RemoveTip()
    {
        GameObject tip = GameObject.FindWithTag("Tip");
        if (tip) Destroy(tip);
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

    private GameObject FindInArray(GameObject[] array, string name, bool nullable = false)
    {
        GameObject obj = Array.Find(array, (o) => o.name == name);

        if (!nullable && obj == null)
            throw new Exception(name + " does not exist in the given array");

        return obj;
    }
}
