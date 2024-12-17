using UnityEngine;

public class UI : MonoBehaviour
{
    public static Canvas canvas;
    public static UI Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

        canvas = gameObject.GetComponent<Canvas>();
    }

    public static void Toggle(bool isEnabled)
    {
        canvas.enabled = isEnabled;
    }
}
