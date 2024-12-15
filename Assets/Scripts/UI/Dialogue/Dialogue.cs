using System;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset file;
    public GameObject character;
    public GameObject cam;
    private static GameObject[] dialogues;

    void Start()
    {
        // var charPos = characterPosition;
        // var charRot = Quaternion.Euler(characterRotation == null ? characterRotation : new Vector3(4.611f, 190.386f, -3.415f));
        // character.transform.SetLocalPositionAndRotation(charPos, charRot);
        // character.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // var camPos = new Vector3(-758.4299f, -430.3781f, -1.259281f);
        // var camRot = Quaternion.Euler(new Vector3(5.087f, -35.723f, -0.001f));
        // cam.transform.SetLocalPositionAndRotation(camPos, camRot);
    }

    public static void Activate(string name)
    {
        Clock.Pause();
        UI.Toggle(false);
        Player.Toggle(false);

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = Array.Find(dialogues, (d) => d.name == name);

        if (dialogue == null)
            throw new Exception(name + " does not exist in Assets/Resources/Dialogues");

        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }
}
