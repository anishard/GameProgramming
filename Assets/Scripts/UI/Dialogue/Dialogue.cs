using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextAsset file;
    public static bool isActive = false;
    private static GameObject[] dialogues;
    private static Dictionary<string, List<GameObject>> npcs;
    private static System.Random rand;

    public static void Activate(string name)
    {
        if (isActive) return;
        
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
        rand ??= new System.Random();

        npcs ??= new Dictionary<string, List<GameObject>>();

        if (!npcs.TryGetValue(name, out var value))
        {
            dialogues ??= Resources.LoadAll<GameObject>("Dialogues");

            IEnumerable<GameObject> matches = dialogues.ToList().Where((d) =>
                d.name.Contains($"{name}Idle")
            );
            
            npcs.Add(name, matches.ToList());
        }

        npcs.TryGetValue(name, out value);
        
        GameObject dialogue = value[rand.Next() % value.Count];

        if (dialogue == null)
            Debug.LogError(name + " does not have any idle dialogues");
            
        var d = Instantiate(dialogue, Vector3.zero, Quaternion.identity);
    }
}
