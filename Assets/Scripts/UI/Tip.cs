using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class Tip : MonoBehaviour
{
    public static bool isActive;
    private static Image backgroundImg;
    private static TMP_Text bodyText;
    private static TextAsset[] tips;

    void Start()
    {
        isActive = false;
        backgroundImg = gameObject.GetComponent<Image>();
        bodyText = transform.Find("Body").GetComponent<TMP_Text>();
    }

    public static void Activate(string name)
    {
        tips ??= Resources.LoadAll<TextAsset>("Tips");

        var lines = new List<string>();

        foreach (var file in tips)
        {
            lines.Clear();
            lines.AddRange(file.text.Split(Environment.NewLine));
            if (lines[0] == name)
                break;
        }

        if (lines[0] == name)
            Tip.Toggle(lines[1]);
        else
            throw new Exception(name + " does not exist in Resources/Tips");
    }

    public static void Remove()
    {
        Toggle();
        Tip.isActive = false;
    }

    public static void Toggle(string body = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(body);

        backgroundImg.enabled = isEnabled;
        bodyText.enabled = isEnabled;
        bodyText.text = body;
        Tip.isActive = true;
    }
}
