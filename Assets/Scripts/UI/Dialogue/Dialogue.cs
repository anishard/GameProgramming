using System;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset file;
    public static bool isActive = false;
    private static GameObject[] dialogues;
    // private static Dictionary<string, game

    public static void Activate(string name)
    {
        Clock.Pause();
        UI.Toggle(false);
        Player.Toggle(false);

        isActive = true;

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

        GameObject dialogue = Array.Find(dialogues, (d) => d.name == name);
        
        if (dialogue == null)
            Debug.LogError(name + " does not exist in Assets/Resources/Dialogues");

        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }

    public static void ActivateNPC(string name, bool isIdle)
    {
        if (!isIdle)
        {
            Activate(name);
            return;
        }

        Clock.Pause();
        UI.Toggle(false);
        Player.Toggle(false);

        isActive = true;

        dialogues ??= Resources.LoadAll<GameObject>("Dialogues/NPC");

        GameObject dialogue = Array.Find(dialogues, (d) => d.name.Contains($"{name}Idle")
        );
        
        if (dialogue == null)
            Debug.LogError(name + " does not exist in Assets/Resources/Dialogues/NPC");

        Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }
}
