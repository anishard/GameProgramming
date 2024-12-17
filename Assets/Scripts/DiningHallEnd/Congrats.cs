using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Congrats : MonoBehaviour
{
    public Canvas congratsUI; 
    public TextMeshProUGUI dishesCountText;  
    public TextMeshProUGUI farmerRatingText; 
    
    void OnTriggerEnter() {
        farmerRatingText.text = "B";
        dishesCountText.text = "25";
        congratsUI.gameObject.SetActive(true);
    }
}