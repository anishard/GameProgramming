using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Note : MonoBehaviour
{
    private static Image backgroundImg;
    private static TMP_Text titleText;
    private static TMP_Text bodyText;
    private static TMP_Text bodyOnlyText;
    private static TextAsset[] notes;

    void Start()
    {
        backgroundImg = gameObject.GetComponent<Image>();
        titleText = transform.Find("Title").GetComponent<TMP_Text>();
        bodyText = transform.Find("Body").GetComponent<TMP_Text>();
        bodyOnlyText = transform.Find("BodyOnly").GetComponent<TMP_Text>();
    }

    public static void Activate(string name)
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
            Note.Toggle(data.title, data.body);
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
