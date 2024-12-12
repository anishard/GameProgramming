using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class Note : MonoBehaviour
{
    private TMP_Text titleObj;
    private TMP_Text textObj;
    private List<string> lines;

    void Start()
    {
        titleObj = transform.Find("Title").GetComponent<TMP_Text>();
        textObj = transform.Find("Text").GetComponent<TMP_Text>();
    }

    public void ToggleNote(string title = "", string text = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(text);

        gameObject.GetComponent<Image>().enabled = isEnabled;
        titleObj.enabled = isEnabled;
        textObj.enabled = isEnabled;

        titleObj.text = title;
        textObj.text = text;
    }
}
