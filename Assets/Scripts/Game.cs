using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour
{
    public static AudioSource audioSource;
    public static AudioClip[] audioClips
    {
        get
        {
            clips ??= Resources.LoadAll<AudioClip>("AudioClips");
            return clips;
        }
    }
    private static AudioClip[] clips;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
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
        AudioClip clip = Array.Find(audioClips, (e) => e.name == clipName);

        if (clip == null)
            throw new Exception(clipName + " does not exist in Resources/AudioClips");

        yield return new WaitForSeconds((float)delay);

        audioSource.PlayOneShot(clip, (float)volumeScale);
    }
}
