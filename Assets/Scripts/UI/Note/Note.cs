using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    private static Image backgroundImg;
    private static TMP_Text titleText;
    private static TMP_Text bodyText;
    private static TMP_Text bodyOnlyText;

    void Start()
    {
        backgroundImg = gameObject.GetComponent<Image>();
        titleText = transform.Find("Title").GetComponent<TMP_Text>();
        bodyText = transform.Find("Body").GetComponent<TMP_Text>();
        bodyOnlyText = transform.Find("BodyOnly").GetComponent<TMP_Text>();
    }

    public static void ToggleNote(string title = "", string body = "")
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
