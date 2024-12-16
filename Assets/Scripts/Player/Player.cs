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

    void Awake()
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
        controller.SetBool("isWalking", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S));

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

        // backward
        if (Input.GetKey(KeyCode.S))
        {
            xdirection *= -1;
            zdirection *= -1;
        }

        float xVal = transform.position.x + xdirection * velocity * Time.deltaTime;
        float zVal = transform.position.z + zdirection * velocity * Time.deltaTime;
        transform.position = new Vector3(xVal, transform.position.y, zVal);

        if (Game.ClickDetected(false) && IsTool(equipped))
        {
            Equippable toEquip = InventoryUI.InteractableDetected() ? Equippable.Interactable : Equippable.None;
            DequipTool(toEquip);
        }

    }

    public static void EquipTool(string itemName)
    {
        Equippable item;

        if (!Enum.TryParse(itemName, out item))
            throw new Exception(itemName + " does not exist in Equippable");

        if (equipped == Equippable.Interactable)
            InventoryUI.DequipInteractable(null);

        if (item == equipped && IsTool(equipped))
        {
            DequipTool(Equippable.None);
            return;
        }

        if (item != equipped || IsTool(equipped) && IsTool(item))
            DequipTool(item);

        if (item != equipped)
        {
            equipped = item;

            foreach (var tool in GameObject.FindGameObjectsWithTag("Tool"))
            {
                if (tool.name == itemName)
                {
                    tool.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            Game.audioSource.PlayOneShot(InventoryUI.equipClip);

            Note.Activate(itemName);
        }
    }

    public static void DequipTool(Equippable toEquip)
    {
        if (!IsTool(equipped)) return;
        
        foreach (var tool in GameObject.FindGameObjectsWithTag("Tool"))
            tool.GetComponent<MeshRenderer>().enabled = false;

        bool noAudio = (toEquip == Equippable.Interactable) // immediately picking up interactable
            || (IsTool(equipped) && IsTool(toEquip)) // switching tools
            || equipped == Equippable.None; // nothing equipped

        if (toEquip != equipped) equipped = Equippable.None;

        if (toEquip == equipped || !noAudio)
            Game.audioSource.PlayOneShot(InventoryUI.dequipClip);

        Note.Remove();
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

    public static bool IsTool(Equippable item)
    {
        return item == Equippable.Can || item == Equippable.Hoe;
    }
}