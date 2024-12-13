using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Note : MonoBehaviour
{
    private TMP_Text titleObj;
    private TMP_Text bodyObj;
    private TMP_Text bodyOnlyObj;

    void Start()
    {
        titleObj = transform.Find("Title").GetComponent<TMP_Text>();
        bodyObj = transform.Find("Body").GetComponent<TMP_Text>();
        bodyOnlyObj = transform.Find("BodyOnly").GetComponent<TMP_Text>();
    }

    public void ToggleNote(string title = "", string body = "")
    {
        bool isEnabled = !string.IsNullOrEmpty(body);
        bool hasTitle = !string.IsNullOrEmpty(title);

        gameObject.GetComponent<Image>().enabled = isEnabled;

        titleObj.enabled = true;
        bodyObj.enabled = isEnabled && hasTitle;
        bodyOnlyObj.enabled = isEnabled && !hasTitle;

        titleObj.text = title;
        if (hasTitle) bodyObj.text = body;
        else bodyOnlyObj.text = body;
    }
}
