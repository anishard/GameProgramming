using UnityEngine;
using UnityEngine.UI;

public class CookingGame : MonoBehaviour {
    public Image meter;
    public Image lowBar;
    public Image highBar;
    public Image container;

    private bool stop;

    void Start() {
        meter.fillAmount = 0.0f;
        stop = false;
    }

    void Update() {
        // Check if user stopped the meter
        if (stop || Input.GetKeyDown(KeyCode.Return)) {
            stop = true;
            return;
        }

        // Increase meter
        if (meter.fillAmount < 1.0f) {
            meter.fillAmount += 0.5f * Time.deltaTime; 
        }

        // Convert fill amount to parent container y-position
        float meterHeight = meter.fillAmount * container.rectTransform.rect.height - (container.rectTransform.rect.height / 2);
        float lowBarHeight = lowBar.rectTransform.localPosition.y;
        float highBarHeight = highBar.rectTransform.localPosition.y;

        // Determine target color based on meter height
        Color targetColor = meter.color;
        if (meterHeight < lowBarHeight) {
            targetColor = Color.yellow;
        }
        else if (meterHeight < highBarHeight) {
            targetColor = new Color(1.0f, 0.64f, 0.0f); 
        }
        else {
            targetColor = Color.red;
        }

        // Lerp the color transition
        meter.color = Color.Lerp(meter.color, targetColor, Time.deltaTime * 6.0f); 
    }
}