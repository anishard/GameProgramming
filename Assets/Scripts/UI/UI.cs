using UnityEngine;

public class UI : MonoBehaviour
{
    public static Canvas canvas;

    void Awake()
    {
        canvas = gameObject.GetComponent<Canvas>();
    }

    public static void Toggle(bool isEnabled)
    {
        canvas.enabled = isEnabled;
    }
}
