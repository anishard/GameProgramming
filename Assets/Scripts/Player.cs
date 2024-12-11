using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocity;
    public float maxVelocity;
    public bool pauseMovement;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    public AudioClip footstepAudio;

    private AudioSource audioSource;
    private Animator controller;
    private Rigidbody rb;

    void Start()
    {
        velocity = 0f;
        maxVelocity = 6f;
        pauseMovement = false;

        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        // stop character from moving
        if (pauseMovement)
        {
            velocity = 0;
            controller.SetBool("isWalking", false);
            controller.SetBool("isJumping", false);
            return;
        }
        
        // rotation
        float horizontal = 0f;

        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        transform.Rotate(Vector3.up, horizontal * 100f * Time.deltaTime);

        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.y);

        // walking
        controller.SetBool("isWalking", Input.GetKey(KeyCode.W));

        if (controller.GetBool("isWalking"))
        {
            if (!audioSource.isPlaying) audioSource.PlayOneShot(footstepAudio, 0.05f);

            velocity += 0.1f; // gradual speed change
            if (velocity > maxVelocity) velocity = maxVelocity;
        }
        else
        {
            velocity = 0.0f;
        }

        // jumping
        controller.SetBool("isJumping", Input.GetKey(KeyCode.Space));

        if (controller.GetBool("isJumping"))
        {
            velocity += 0.1f; // gradual speed change
            if (velocity > 6f) velocity = 6f;
        }

        // keep character within bounds
        float xVal = transform.position.x + xdirection * velocity * Time.deltaTime;
        float zVal = transform.position.z + zdirection * velocity * Time.deltaTime;

        if (minBounds != Vector3.zero)
        {
            if (xVal <= minBounds.x) xVal = minBounds.x;
            if (zVal <= minBounds.z) zVal = minBounds.z;
        }
        if (maxBounds != Vector3.zero)
        {
            if (xVal >= maxBounds.x) xVal = maxBounds.x;
            if (zVal >= maxBounds.z) zVal = maxBounds.z;
        }

        transform.position = new Vector3(xVal, transform.position.y, zVal);
    }
}