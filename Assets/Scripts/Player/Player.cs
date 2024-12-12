using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public GameObject activeArea;
    public Equippable equipped;
    public float velocity;
    public float maxVelocity;
    public bool pauseMovement;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    public AudioClip footstepAudio;

    private Game game;
    private AudioSource audioSource;
    private Animator controller;
    private Rigidbody rb;

    //ADDED CODE FOR INVENTORY
    Camera cam;

    public Interactable focus;
//     Interactable interact = new Interactable();
// MyScript scriptInstance = newObject.AddComponent<MyScript>()
    void Start()
    {
        activeArea = GameObject.Find("ActiveArea");
        //ADDED CODE FOR INVENTORY
        cam = Camera.main;

        equipped = Equippable.None;
        velocity = 0f;
        maxVelocity = 6f;
        pauseMovement = false;

        game = GameObject.Find("GameManager").GetComponent<Game>();
        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        // check if hovering over UI (inventory), if so then don't move player 
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
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

        //ADDED CODE FOR PICKING UP STUFF FOR INVENTORY - Claire
        // if (Input.GetMouseButtonDown(1)) {
        //     RemoveFocus();
        // }
        if (Input.GetMouseButtonDown(1)) {
            RemoveFocus();
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100)) {
                // Check if we hit an interactable
                //  if we did, log that we found an interactable object
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                //GameObject obj = hit.collider.GetComponent<GameObject>();
                
                if (interactable != null) {
                    // SetFocus(interactable);
                    Debug.Log("Found an interactable object");
                    //Destroy(obj);
                    SetFocus(interactable);
                }
            }
        }
    }  

    void SetFocus (Interactable newFocus) {
        if (newFocus != focus) {
            if (focus != null) {
                focus.OnDefocused();
            }
            focus = newFocus;
        }
        newFocus.OnFocused(transform); 
    }  

    void RemoveFocus() {
        if (focus != null) {
            focus.OnDefocused();
        }
        focus = null;
    }

    public void Equip(string itemName)
    {
        Equippable item;

        if (!Enum.TryParse(itemName, out item))
            throw new Exception(itemName + " does not exist in enum Equippable");

        bool notAlreadyEquipped = item != equipped;

        equipped = notAlreadyEquipped ? item : Equippable.None;

        foreach (var tool in GameObject.FindGameObjectsWithTag("Tool"))
        {
            var renderer = tool.GetComponent<MeshRenderer>();
            if (tool.name == itemName) renderer.enabled = notAlreadyEquipped;
            else renderer.enabled = false;
        }

        game.RemoveTip();
        if (notAlreadyEquipped) game.ActivateTip(itemName);
    }

    public bool ObjectDetected(string objectName)
    {
        Collider[] colliders = Physics.OverlapSphere(
            activeArea.transform.position, 0.75f
        );

        return Array.Find(colliders, (c) => c.name == objectName);
    }
}