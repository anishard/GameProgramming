using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    private Inventory inventory;

    void Start() {
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();
    }

    public void Test() {
        Debug.Log("Click caputed!");
    }
}
