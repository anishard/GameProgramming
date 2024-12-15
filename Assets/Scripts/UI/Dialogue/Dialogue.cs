using System;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset file;
    public GameObject character;
    public GameObject cam;
    public Vector3 characterPosition = new Vector3(-751.834f, -430.879f, 6.863f);
    public Vector3 characterRotation;
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    private static GameObject[] dialogues;

    void Start()
    {
        var charPos = characterPosition;
        var charRot = Quaternion.Euler(characterRotation == null ? characterRotation : new Vector3(4.611f, 190.386f, -3.415f));
        character.transform.SetLocalPositionAndRotation(charPos, charRot);
        character.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        var camPos = cameraPosition == null ? cameraPosition : new Vector3(-750.4415f, -430.833f, 3.779298f);
        var camRot = Quaternion.Euler(cameraRotation == null ? cameraRotation : new Vector3(-4.196f, -43.441f, -0.006f));
        cam.transform.SetLocalPositionAndRotation(camPos, camRot);
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
