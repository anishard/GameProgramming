using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class Note : MonoBehaviour
{
    private TMP_Text titleObj;
    private TMP_Text textObj;
    private TMP_Text textOnlyObj;

    void Start()
    {
        titleObj = transform.Find("Title").GetComponent<TMP_Text>();
        textObj = transform.Find("Text").GetComponent<TMP_Text>();
        textOnlyObj = transform.Find("TextOnly").GetComponent<TMP_Text>();
    }

    public void ToggleNote(string title = "", string text = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(text);
        bool isTextOnly = string.IsNullOrEmpty(title);

        gameObject.GetComponent<Image>().enabled = isEnabled;
        titleObj.enabled = isEnabled;
        textObj.enabled = !isTextOnly;
        textOnlyObj.enabled = isTextOnly;

        titleObj.text = title;
        if (!isTextOnly) textObj.text = text;
        else textOnlyObj.text = text;
    }
}
