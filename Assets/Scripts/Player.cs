using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocity;
    public float maxVelocity;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    private Animator controller;
    private Rigidbody rb;

    void Start()
    {
        velocity = 0f;
        maxVelocity = 6f;
        controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float horizontal = 0f;

        // rotate character
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        transform.Rotate(Vector3.up, horizontal * 100f * Time.deltaTime);

        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);

        // get character to walk
        controller.SetBool("isWalking", Input.GetKey(KeyCode.W));

        if (controller.GetBool("isWalking") == true)
        {
            // gradual speed change
            velocity += 0.1f;
            if (velocity > maxVelocity) velocity = maxVelocity;
        }
        else
        {
            velocity = 0.0f;
        }

        // get character to jump
        controller.SetBool("isJumping", Input.GetKey(KeyCode.Space));

        // get character to walk when click up arrow
        if (controller.GetBool("isJumping") == true)
        {
            // gradual speed change
            velocity += 0.1f;
            if (velocity > 6f) velocity = 6f;
        }

        // keep character within bounds
        float xVal = transform.position.x + xdirection * velocity * Time.deltaTime;
        float zVal = transform.position.z + zdirection * velocity * Time.deltaTime;

        if (minBounds != Vector3.zero) {
            if (xVal <= minBounds.x) xVal = minBounds.x;
            if (zVal <= minBounds.z) zVal = minBounds.z;
        }
        if (maxBounds != Vector3.zero) {
            if (xVal >= maxBounds.x) xVal = maxBounds.x;
            if (zVal >= maxBounds.z) zVal = maxBounds.z;
        }

        transform.position = new Vector3(xVal, transform.position.y, zVal);
    }
}