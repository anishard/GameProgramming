using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    public string title;
    public string text;
    public static readonly Vector3 position = new(640 - 438f, 360 - 270f, 0f);

    void Start()
    {
        transform.Find("Title").GetComponent<TMP_Text>().text = title;
        transform.Find("Text").GetComponent<TMP_Text>().text = text;
    }
}
