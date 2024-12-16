using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private int step = 1;

    void Start()
    {
        Dialogue.Activate("Tutorial1");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
