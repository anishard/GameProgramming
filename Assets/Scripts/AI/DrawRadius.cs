using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRadius : MonoBehaviour
{
    public int radius;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
