using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventSceneChange : MonoBehaviour
{
    Collider objCollider;
    // Start is called before the first frame update
    void Start()
    {
        objCollider = GetComponent<Collider>();
        objCollider.isTrigger = true;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void preventTriggerSceneChange() {
        objCollider.isTrigger = false;
    }

    public void enableTriggerSceneChange() {
        objCollider.isTrigger = true;
    }
}
