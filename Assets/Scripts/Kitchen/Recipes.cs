using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public TextMeshProUGUI hint; 

    private Inventory inventory;

    void Start() 
    {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();


        Item meat = (Item)ScriptableObject.CreateInstance("Item");
        meat.name = "Meat";
        inventory.Add(meat);
    }

    Item FindItem(string name) 
    {
        // Check if player has item of given name and amount
        foreach (Item item in inventory.items) {
            if (item.name == name) {
                return item;
            }
        }   
        return null;
    }

    void ShowHint() 
    {
        hint.text = "You don't have the required ingredients to cook that!";
        hint.gameObject.SetActive(true); 
        Invoke(nameof(HideHint), 1f); 
    }

    void HideHint()
    {
        hint.gameObject.SetActive(false); 
    }

    public void Test() 
    {
        Debug.Log("Click caputed!");
    }

    public void SteakRecipe() 
    {
        Item meat = FindItem("Meat");
        if (meat != null) {
            inventory.Remove(meat);
        }
        else {
            ShowHint();
        }
    }
}
