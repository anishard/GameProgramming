using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float velocity;
    public float maxVelocity;
    public Vector3 minBounds;
    public Vector3 maxBounds;
    public AudioClip footstepAudio;

    public static Transform playerTransform;
    public static GameObject activeArea;
    public static Equippable equipped;
    public static bool pauseMovement;

    private AudioSource audioSource;
    private Animator controller;
    private Rigidbody rb;

    void Start()
    {
        activeArea = GameObject.Find("ActiveArea");

        equipped = Equippable.None;
        velocity = 0f;
        maxVelocity = 6f;
        pauseMovement = false;

        playerTransform = gameObject.transform;
        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        // check if hovering over UI (inventory), if so then don't move player 
        if (EventSystem.current.IsPointerOverGameObject())
        {
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
            velocity += 0.1f;
            if (velocity > 6f) velocity = 6f;
        }

        float xVal = transform.position.x + xdirection * velocity * Time.deltaTime;
        float zVal = transform.position.z + zdirection * velocity * Time.deltaTime;
        transform.position = new Vector3(xVal, transform.position.y, zVal);

        if (Input.GetKeyDown("space")) SceneManager.LoadScene("DiningRoom");
    }

    public static void EquipTool(string itemName)
    {
        if (equipped == Equippable.Interactable) return;
        
        Equippable item;

        if (!Enum.TryParse(itemName, out item))
            throw new Exception(itemName + " does not exist in Equippable");

        bool notAlreadyEquipped = (item != equipped);

        equipped = notAlreadyEquipped ? item : Equippable.None;

        foreach (var tool in GameObject.FindGameObjectsWithTag("Tool"))
        {
            var renderer = tool.GetComponent<MeshRenderer>();
            if (tool.name == itemName) renderer.enabled = notAlreadyEquipped;
            else renderer.enabled = false;
        }

        Note.Remove();
        if (notAlreadyEquipped) Note.Activate(itemName);
    }

    public static bool ObjectDetected(string objectName)
    {
        Collider[] colliders = Physics.OverlapSphere(
            activeArea.transform.position, 0.75f
        );

        return Array.Find(colliders, (c) => c.name == objectName);
    }

    public static IEnumerator Pause(Action callback, float seconds = 1)
    {
        pauseMovement = true;
        yield return new WaitForSeconds(seconds);
        callback();
        pauseMovement = false;
    }

    public static void Toggle(bool isEnabled)
    {
        Transform[] children = GameObject
            .Find("Player")
            .GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            var renderer = child.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (renderer != null) renderer.enabled = isEnabled;
        }
    }
}