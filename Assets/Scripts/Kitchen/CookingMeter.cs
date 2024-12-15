using UnityEngine;
using UnityEngine.UI;
using System.Collections;  
using TMPro;

public class CookingMeter : MonoBehaviour {
    public Image meter;
    public Image lowBar;
    public Image highBar;
    public Image container;
    public TextMeshProUGUI countdownText;  

    private bool stop;
    private const float barDistance = 30.0f;
    private float fillSpeed;
    private bool countdownFinished = false; 
    private KitchenGame kitchenGame;

    void Start() {
        kitchenGame = FindObjectOfType<KitchenGame>();
        meter.fillAmount = 0.0f;
        stop = false;

        // Randomize fill speed and bar heights
        fillSpeed = Random.Range(0.5f, 1.5f);

        float containerHeight = container.rectTransform.rect.height;
        float minHeight = containerHeight * 0.4f;
        float maxHeight = containerHeight * 0.9f;

        // Randomize lowBar position
        float randomizedLowBarHeight = Random.Range(minHeight, maxHeight);
        lowBar.rectTransform.localPosition = new Vector3(
            lowBar.rectTransform.localPosition.x,
            randomizedLowBarHeight - (containerHeight / 2),
            lowBar.rectTransform.localPosition.z
        );

        // Position highBar at a fixed height above lowBar
        highBar.rectTransform.localPosition = new Vector3(
            highBar.rectTransform.localPosition.x,
            lowBar.rectTransform.localPosition.y + barDistance,
            highBar.rectTransform.localPosition.z
        );

        // Start the countdown coroutine
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine() {
        int countdownTime = 3;
        while (countdownTime > 0) {
            if (countdownText != null) {
                countdownText.text = countdownTime.ToString();  
            }
            yield return new WaitForSeconds(1); 
            countdownTime--;
        }

        // After countdown finishes, start filling the meter
        countdownFinished = true;
        if (countdownText != null) {
            countdownText.text = "Go!";  
        }
    }

    IEnumerator EndGame() {
        // Lets the meter linger for a second
        stop = true; 
        kitchenGame.StartNextTodo();
        yield return new WaitForSeconds(1f);
        kitchenGame.meterGameUI.gameObject.SetActive(false);
    }

    void Update() {
        // Wait until the countdown finishes before starting 
        if (!countdownFinished) return;

        // Increase meter
        if (meter.fillAmount < 1.0f && !stop) {
            meter.fillAmount += fillSpeed * Time.deltaTime;
        }

        // Convert fill amount to parent container y-position
        float meterHeight = meter.fillAmount * container.rectTransform.rect.height - (container.rectTransform.rect.height / 2);
        float lowBarHeight = lowBar.rectTransform.localPosition.y;
        float highBarHeight = highBar.rectTransform.localPosition.y;

        // Determine target color and cooking quality based on meter height
        Color targetColor;
        if (meterHeight < lowBarHeight) {
            kitchenGame.cookQuality = "Undercooked";
            targetColor = Color.yellow;
        }
        else if (meterHeight <= highBarHeight) {
            kitchenGame.cookQuality = "Perfect";
            targetColor = new Color(1.0f, 0.64f, 0.0f); // Orange
        }
        else {
            kitchenGame.cookQuality = "Overcooked";
            targetColor = Color.red;
        }

        // Stop the meter when the player inputs
        if (!stop && Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(EndGame()); 
            return;
        }

        // Lerp the color transition
        meter.color = Color.Lerp(meter.color, targetColor, Time.deltaTime * 6.0f);
    }
}