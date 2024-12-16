using UnityEngine;
using UnityEngine.UI;
using System.Collections;  
using TMPro;
using System.Threading;

public class CookingMeter : MonoBehaviour {
    public Image meter;
    public Image lowBar;
    public Image highBar;
    public TextMeshProUGUI countdownText;  

    private const float barDistance = 30.0f;
    private float fillSpeed;
    private bool countdownFinished = false; 
    private KitchenGame kitchenGame;

    void Start() {
        kitchenGame = FindObjectOfType<KitchenGame>();
    }

    public void Play() {
        this.enabled = true;
        meter.fillAmount = 0.0f;
        countdownFinished = false;

        // Randomize fill speed and bar heights
        fillSpeed = Random.Range(0.5f, 1.5f);

        float meterHeight = meter.rectTransform.rect.height;
        float minHeight = meterHeight * 0.4f;
        float maxHeight = meterHeight * 0.9f;

        // Randomize lowBar position
        float randomizedLowBarHeight = Random.Range(minHeight, maxHeight);
        lowBar.rectTransform.localPosition = new Vector3(
            lowBar.rectTransform.localPosition.x,
            randomizedLowBarHeight - (meterHeight / 2),
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

    public void Stop() {
        this.enabled = false;
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
        countdownText.text = "Go!";  
    }

    IEnumerator DelayedClose() {
        // Lets the meter linger for a second
        Stop();
        kitchenGame.StartNextTodo();
        yield return new WaitForSeconds(1f);
        kitchenGame.meterGameUI.gameObject.SetActive(false);
    }

    void Update() {
        // Wait until the countdown finishes before starting 
        if (!countdownFinished) {
            return;
        }

        // Stop the meter when the player inputs
        if (Input.GetKeyDown(KeyCode.Return) || meter.fillAmount >= 1.0f) {
            StartCoroutine(DelayedClose()); 
            return;
        }

        // Increase meter
        if (meter.fillAmount < 1.0f) {
            meter.fillAmount += fillSpeed * Time.deltaTime;
        }

        // Convert fill amount to meter y-position
        float meterHeight = meter.fillAmount * meter.rectTransform.rect.height - (meter.rectTransform.rect.height / 2);
        float lowBarHeight = lowBar.rectTransform.localPosition.y;
        float highBarHeight = highBar.rectTransform.localPosition.y;

        // Determine target color and cooking quality based on meter height
        Color targetColor;
        if (meterHeight < lowBarHeight) {
            kitchenGame.cookQuality = "undercooked";
            targetColor = Color.yellow;
        }
        else if (meterHeight <= highBarHeight) {
            kitchenGame.cookQuality = "perfect";
            targetColor = new Color(1.0f, 0.64f, 0.0f); // Orange
        }
        else {
            kitchenGame.cookQuality = "overcooked";
            targetColor = Color.red;
        }

        // Lerp the color transition
        meter.color = Color.Lerp(meter.color, targetColor, Time.deltaTime * 6.0f);
    }
}