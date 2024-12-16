using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class Note : MonoBehaviour
{
    private static Image backgroundImg;
    private static TMP_Text titleText;
    private static TMP_Text bodyText;
    private static TMP_Text bodyOnlyText;
    private static IEnumerable<string> notes;
    private static System.Random rand;

    void Start()
    {
        backgroundImg = gameObject.GetComponent<Image>();
        titleText = transform.Find("Title").GetComponent<TMP_Text>();
        bodyText = transform.Find("Body").GetComponent<TMP_Text>();
        bodyOnlyText = transform.Find("BodyOnly").GetComponent<TMP_Text>();
    }

    public static void Activate(string name)
    {
        notes ??= Resources.LoadAll<TextAsset>("Notes").Select((n) => n.text);
        rand ??= new System.Random();

        List<NoteData> matches = new();

        foreach (var note in notes)
        {
            var json = JsonUtility.FromJson<NoteData>(note);
            
            if (
                name == json.objectName ||
                name.Contains("(Clone)") && name[..^7] == json.objectName
            )
                matches.Add(json);
        }

        if (matches.Count > 0)
        {
            var note = matches[rand.Next() % matches.Count];
            Toggle(note.title, note.body);
        }
        else
            throw new Exception(name + " does not exist in Resources/Notes");
    }

    public static void Remove()
    {
        Toggle();
    }

    public static void Toggle(string title = "", string body = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(body);
        bool hasTitle = !string.IsNullOrEmpty(title);

        backgroundImg.enabled = isEnabled;

        titleText.enabled = true;
        bodyText.enabled = isEnabled && hasTitle;
        bodyOnlyText.enabled = isEnabled && !hasTitle;

        titleText.text = title;
        if (hasTitle) bodyText.text = body;
        else bodyOnlyText.text = body;
    }
}
