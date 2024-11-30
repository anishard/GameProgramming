using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalk : MonoBehaviour
{
    //for the character
    public Animator controller;
    public float velocity;

    //add rigidbody for collisions
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
     velocity = 0f;  
     rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {   
        float horizontal = 0f;

        //rotate character
        if (Input.GetKey(KeyCode.LeftArrow)) {
            // transform.Rotate(new Vector3(0.0f, -1f, 0.0f));
            //rotate controller
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            // transform.Rotate(new Vector3(0.0f, 1f, 0.0f));
            horizontal = 1f;
        }

        // float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        // float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        transform.Rotate(Vector3.up, horizontal * 100f * Time.deltaTime);

        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);
        Vector3 moveDirection = new Vector3(xdirection, 0.0f, zdirection).normalized;

        //code for character walking
        if (Input.GetKey(KeyCode.UpArrow)) {
           controller.SetBool("isWalking", true); 
        }
        else {
           controller.SetBool("isWalking", false);
        }


        //get character to walk when click up arrow
        if (controller.GetBool("isWalking") == true) {
            //gradual speed change
            Debug.Log("iswalking is true so vel should be triggered");
            velocity += 0.1f;
            Debug.Log(velocity); //vel is working
            if (velocity > 3f) {
                velocity = 3f;
            }
        }
        else {
            velocity = 0.0f;
        }

        //get character to jump
        if (Input.GetKey(KeyCode.Space)) {
           controller.SetBool("isJumping", true); 
        }
        else {
           controller.SetBool("isJumping", false);
        }


        //get character to walk when click up arrow
        if (controller.GetBool("isJumping") == true) {
            //gradual speed change
            Debug.Log("isjumping should be triggered");
            velocity += 0.1f;
            if (velocity > 5f) {
                velocity = 5f;
            }
        }
        
        //to keep character on path
        float xVal = transform.position.x + xdirection * velocity * Time.deltaTime;

        if (xVal >= 387.43f) {
            xVal = 387.43f;
        }

        if (xVal <= 385.53f) {
            xVal = 385.53f;
        }

        float zVal = transform.position.z + zdirection * velocity * Time.deltaTime;
        //make sure character doesn't go too far
        if (zVal <= 235.7f) {
            zVal = 235.7f;
        }

        if (zVal >= 280f)
        {
            zVal = 280f;
        }
        transform.position = new Vector3(xVal, transform.position.y, zVal);
        
    }
}