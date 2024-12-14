using UnityEngine;
using UnityEngine.UI;

public class MeterMiniGame : MonoBehaviour {
    public Image meter;
    public Image lowBar;
    public Image highBar;
    public Image container;

    private bool stop;
    private const float barDistance = 30.0f; 
    private float fillSpeed;

    void Start() {
        meter.fillAmount = 0.0f;
        stop = false;

        // Randomize fill speed
        fillSpeed = Random.Range(0.5f, 1.5f);

        // Randomize lowBar height
        float containerHeight = container.rectTransform.rect.height;
        float minHeight = containerHeight * 0.3f; 
        float maxHeight = containerHeight * 0.9f; 

        // Set randomized lowBar position
        float randomizedLowBarHeight = Random.Range(minHeight, maxHeight);
        lowBar.rectTransform.localPosition = new Vector3(
            lowBar.rectTransform.localPosition.x,
            randomizedLowBarHeight - (containerHeight / 2), 
            lowBar.rectTransform.localPosition.z
        );

        // Position highBar at a fixed distance above lowBar
        highBar.rectTransform.localPosition = new Vector3(
            highBar.rectTransform.localPosition.x,
            lowBar.rectTransform.localPosition.y + barDistance,
            highBar.rectTransform.localPosition.z
        );
    }

    void Update() {
        // Check if user stopped the meter
        if (stop || Input.GetKeyDown(KeyCode.Return)) {
            stop = true;
            return;
        }

        // Increase meter
        if (meter.fillAmount < 1.0f) {
            meter.fillAmount += fillSpeed * Time.deltaTime;
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
            targetColor = new Color(1.0f, 0.64f, 0.0f); // Orange
        }
        else {
            targetColor = Color.red;
        }

        // Lerp the color transition
        meter.color = Color.Lerp(meter.color, targetColor, Time.deltaTime * 6.0f);
    }
}