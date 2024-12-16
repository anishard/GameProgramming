using System;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset file;
    private static GameObject[] dialogues;

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
