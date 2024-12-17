using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Congrats : MonoBehaviour
{
    public Canvas congratsUI; 
    public TextMeshProUGUI dishesCountText;  
    public TextMeshProUGUI farmerRatingText; 
    
    void OnTriggerEnter() {
        int count = DishCount.count;
        if (count >= 30) {
            farmerRatingText.text = "A";
        }
        else if (count >= 20) {
            farmerRatingText.text = "B";
        }
        else {
            farmerRatingText.text = "C";
        }
        dishesCountText.text = "" + DishCount.count;
        congratsUI.gameObject.SetActive(true);
    }
}