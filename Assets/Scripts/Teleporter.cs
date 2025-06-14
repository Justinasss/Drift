using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportDestination;
    [Tooltip("How long the player is frozen after teleporting (in seconds)")]
    public float freezeDuration = 1.0f;

    [Header("UI References")]
    public TextMeshProUGUI failsCounterText; // Assign your UI Text (TMP) element here

    [Header("Car Controller Reference")]
    [Tooltip("Assign your car's main controller script here.")]
    public MonoBehaviour carController; // Using MonoBehaviour to be flexible, cast later

    // Private variables
    private Rigidbody rb;
    private int failCount = 0;
    private bool isTeleporting = false; // Prevents multiple teleports if collision persists

    void Start()
    {
        // Get the Rigidbody component on this GameObject when the script starts
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Teleporter script requires a Rigidbody component on the same GameObject!", this);
            enabled = false; // Disable the script if no Rigidbody is found
        }

        // Initialize the UI text
        UpdateFailsCounterUI();

        if (carController == null)
        {
            Debug.LogError("Car Controller not assigned on Teleporter script! Teleport freeze and grip reset will not work.", this);
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Terrain") && !isTeleporting)
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        isTeleporting = true; 
       
        failCount++;
        UpdateFailsCounterUI(); 

        
        transform.position = teleportDestination.position;

        
        transform.rotation = Quaternion.identity; 

        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;             
            rb.angularVelocity = Vector3.zero;      
        }

        Debug.Log($"Player/Car collided with terrain and teleported. Fails: {failCount}");

        
        StartCoroutine(HandleTeleportEffects());
    }

    // Coroutine to manage the freeze and communicate with the CarController
    private IEnumerator HandleTeleportEffects()
    {
        if (carController != null)
        {
            
            carController.SendMessage("SetInputEnabled", false, SendMessageOptions.DontRequireReceiver);

            carController.SendMessage("ResetGripForTeleport", SendMessageOptions.DontRequireReceiver);
        }
       

        
        yield return new WaitForSeconds(freezeDuration);

        
        if (carController != null)
        {
            carController.SendMessage("SetInputEnabled", true, SendMessageOptions.DontRequireReceiver);
        }

        isTeleporting = false; 
    }

    // Helper function to update the UI text
    void UpdateFailsCounterUI()
    {
        if (failsCounterText != null)
        {
            failsCounterText.text = $"Fails: {failCount}";
        }
        
    }
}
