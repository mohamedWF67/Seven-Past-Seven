using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PortalLogic : MonoBehaviour
{
    private HashSet<GameObject> portalObjects =  new ();
    
    [Tooltip("Sets the portal to teleport to.")]
    [SerializeField] Transform destinationPortal;
    
    [Tooltip("Toogle for if portal imediately teleports the player or only when pressing the up arrow.")]
    [SerializeField] private bool isSideTeleport;
    
    [Tooltip("The index of the door in the portal array.")]
    [SerializeField] private int doorIndex;

    [Tooltip("If the portal is unlocked.")] 
    [SerializeField] private bool isUnlocked;
    
    private Collider2D player;
    private InputAction portalInput;
    public Coroutine teleportingCoroutine;
    [SerializeField]bool isCoroutineRunning;
    
    private void Start()
    {
        portalInput = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Teleport");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        //* Unlocks the portal when the player collects the key.
        if(other.GetComponent<KeyHolder>().CheckIfKeyCollected(doorIndex) && !isUnlocked)
        {
            isUnlocked = true;
            destinationPortal.gameObject.GetComponent<PortalLogic>().isUnlocked = true;
            Debug.Log("Door Unlocked");
            NotificationTextScript.instance.ShowNotification("Door Unlocked" + "\n" + "Press W or R3 to Teleport");
        }
        
        //* Prevents the player from teleporting infinitely.
        if (portalObjects.Contains(other.gameObject)) return;
        //* Does the actual teleportation logic.
        if (!isSideTeleport && teleportingCoroutine == null && !isCoroutineRunning)
        {
            teleportingCoroutine = StartCoroutine(Teleport(other.gameObject));
        }
        player = other;
    }

    private void Update()
    {
        if (player == null) return;
        if (!player.CompareTag("Player")) return;

        //* Removes the player when depressing the door button.
        if (portalInput.IsPressed() && portalObjects.Contains(player.gameObject) && isSideTeleport)
        {
            portalObjects.Remove(player.gameObject);
        }
        
        //* Prevents the player from teleporting infinitely.
        if (portalObjects.Contains(player.gameObject)) return;
        
        //* Teleports the player when pressing the door button.
        if (portalInput.triggered && isSideTeleport && teleportingCoroutine == null && !isCoroutineRunning)
        {
            //Debug.Log("Starting teleport");
            teleportingCoroutine = StartCoroutine(Teleport(player.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        //* Removes the player when leaving the portal.
        portalObjects.Remove(other.gameObject);
        player = null;
    }

    IEnumerator Teleport(GameObject other)
    {
        if (!isUnlocked)
        {
            Debug.Log($"Needs Key {doorIndex} to unlock.");
            NotificationTextScript.instance.ShowNotification($"Needs Key {doorIndex} to unlock.");
            teleportingCoroutine = null;
            yield break;
        }
        isCoroutineRunning = true;
        FullScreenEffectScript.instance.Blink();
        if (destinationPortal.TryGetComponent(out PortalLogic portal))
            portal.isCoroutineRunning = true;
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        //* Adds the player to the other portal's set.
        portal.portalObjects.Add(other.gameObject);
        portal.player = player;
        //* Teleports the player.
        other.transform.position = destinationPortal.position;
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        //Debug.Log("Ready for teleport");
        isCoroutineRunning = false;
        portal.isCoroutineRunning = false;
        teleportingCoroutine = null;
    }
}
