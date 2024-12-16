using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour {
    public static int walletBalance = 100;

    public TextMeshProUGUI walletText;

    void Start() {
        walletText.text = "Coins: " + walletBalance;
    }

    public void ChangeBalance(int balChange) {
        walletBalance += balChange;
        walletText.text = "Coins: " + walletBalance;
    }
}