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
    private static float activateTime;
    private static float duration;

    void Start()
    {
        isActive = false;
        backgroundImg = gameObject.GetComponent<Image>();
        bodyText = transform.Find("Body").GetComponent<TMP_Text>();

        activateTime = 0;
        duration = 0;
    }

    void Update()
    {
        if (duration > 0 && Time.time - activateTime > duration)
            Remove();
    }

    public static void Activate(string name, float tipDuration = 0)
    {
        Remove();

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
        {
            if (tipDuration > 0) duration = tipDuration;
            Toggle(lines[1]);
        }
        else
            throw new Exception(name + " does not exist in Resources/Tips");
    }

    public static void Remove()
    {
        Toggle();
    }

    public static void Toggle(string body = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(body);

        backgroundImg.enabled = isEnabled;
        bodyText.enabled = isEnabled;
        bodyText.text = body;

        isActive = isEnabled;
        activateTime = isEnabled ? Time.time : 0;
        duration = isEnabled ? duration : 0;
    }
}
