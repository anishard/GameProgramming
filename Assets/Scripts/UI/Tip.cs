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

    void Awake()
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

    public static void Activate(string body, float tipDuration = 0)
    {
        if (tipDuration > 0) duration = tipDuration;

        Remove();
        Toggle(body);
    }

    public static void Remove()
    {
        Debug.Log(Time.time + "remove");
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
