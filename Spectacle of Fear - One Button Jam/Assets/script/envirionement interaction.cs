using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class envirionementinteraction : MonoBehaviour
{
    public GameObject lock_text; // Reference to the lock text GameObject
    public bool locked = false; // Flag to indicate if the door is locked
    public enum InteractionType
    {
        None,
        easter_egg,
        key,
        door,
        stair,
        ForceMove
    }
    public bool disable_first_floor = false;
    public bool enable_first_floor = false;
    public GameObject first_floor;
    private bool isopen = false;
    public GameObject player;
    public actionmanager action_manager;
    public GameObject interactableObject;
    public GameObject door; // Reference to the door GameObject
    public InteractionType currentInteractionType = InteractionType.None;
    public enum ddirection //door direction north, east, south, west
    {
        north,
        east,
        south,
        west
    }
    public ddirection currentddirection = ddirection.north;

    public void Awake()
    {
        action_manager = FindAnyObjectByType<actionmanager>();
        player = GameObject.FindGameObjectWithTag("GameController");
        if (lock_text != null)
        {
            lock_text.SetActive(false); // Ensure the lock text is hidden at the start
        }
    }
    
    public void interact(InteractionType InteractionType)
    {
        if (action_manager.isinteracting)
        {
            Debug.Log("Interacting with: " + interactableObject.name);
            switch (InteractionType)
            {
                case InteractionType.easter_egg:
                    // Handle easter egg interaction
                    // Trigger easter egg event or animation
                    Debug.Log("Interacted with an easter egg!");
                    break;
                case InteractionType.key:
                    // Handle key interaction
                    //add key to inventory
                    Debug.Log("Picked up a key!");
                    break;
                case InteractionType.door:
                    // Handle door interaction
                    switch (player.transform.rotation.eulerAngles.y)
                    {
                        case 0:
                            if (currentddirection == ddirection.west)
                            {
                                // Open the door facing north
                                Debug.Log("Opened a door facing west!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 90:
                            if (currentddirection == ddirection.north)
                            {
                                // Open the door facing east
                                Debug.Log("Opened a door facing north!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 180:
                            if (currentddirection == ddirection.east)
                            {
                                // Open the door facing south
                                Debug.Log("Opened a door facing east!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 270:
                            if (currentddirection == ddirection.south)
                            {
                                // Open the door facing west
                                Debug.Log("Opened a door facing south!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                    }
                    if(!locked)
                    {
                    StartCoroutine(opendoor()); // Call the method to open the door
                    }
                    else
                    {
                        lock_text.SetActive(true);
                        StartCoroutine(HideLockTextAfterDelay(4f)); // Hide the lock text after 4 seconds
                        Debug.Log("The door is locked!");
                    }
                    break;
                    case InteractionType.stair:
                    // Handle stair interaction
                    switch (player.transform.rotation.eulerAngles.y)
                    {
                                            case 0:
                            if (currentddirection == ddirection.west)
                            {
                                // Open the door facing north
                                Debug.Log("used stair facing west!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 90:
                            if (currentddirection == ddirection.north)
                            {
                                // Open the door facing east
                                Debug.Log("used stair facing north!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 180:
                            if (currentddirection == ddirection.east)
                            {
                                // Open the door facing south
                                Debug.Log("used stair facing east!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 270:
                            if (currentddirection == ddirection.south)
                            {
                                // Open the door facing west
                                Debug.Log("used stair facing south!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                    }
                    travel_stair();
                    break;
                case InteractionType.ForceMove:
                    switch (player.transform.rotation.eulerAngles.y)
                    {
                        case 0:
                            if (currentddirection == ddirection.west)
                            {
                                // Open the door facing north
                                Debug.Log("force moved facing west!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 90:
                            if (currentddirection == ddirection.north)
                            {
                                // Open the door facing east
                                Debug.Log("force moved facing north!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 180:
                            if (currentddirection == ddirection.east)
                            {
                                // Open the door facing south
                                Debug.Log("force moved facing east!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                        case 270:
                            if (currentddirection == ddirection.south)
                            {
                                // Open the door facing west
                                Debug.Log("force moved facing south!");
                            }
                            else
                            {
                                return; // Do nothing if the player is not facing the correct direction
                            }
                            break;
                    }
                    action_manager.forcemove();
                    break;
                default:
                    Debug.Log("No interaction defined for this type.");
                    break;
            }
        }
    }
    private IEnumerator opendoor()
    {
        if (isopen) yield break; // Exit if the door is already open
        door.transform.Rotate(0, 85, 0); // Rotate the door by ~85 degrees to open it
        isopen = true; // Set the door state to open
        yield return new WaitForSeconds(0.5f); // Wait for 1 second before closing moving the player forward
        action_manager.forcemove(); // Move the player forward when interacting with a door
        door.transform.Rotate(0, -85, 0); // Rotate the door back to its original position after opening
        isopen = false; // Set the door state back to closed
        Debug.Log("Opened a door!");
    }
    private void travel_stair()
    {
        // Implement the logic to move the player to the next floor or area
        Debug.Log("Traveling up the stairs!");
        if (currentddirection == ddirection.north)
        {
            // Move the player up by 5 units in the Y-axis
            player.transform.position += new Vector3(18f, 6.1f, 0f);
        }
        else
        {
            // Move the player up by 5 units in the Y-axis
            player.transform.position += new Vector3(-18f, -6.1f, 0f);
        }
    }
     // Fires the exact frame another collider enters the trigger zone
    private void OnTriggerStay(Collider other)
    {
        if (disable_first_floor)
        {
            first_floor.SetActive(false);
        }
        if (enable_first_floor)
        {
            first_floor.SetActive(true);
        }
        // Check if the entering object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            interact(currentInteractionType); // Call the interact method with the current interaction type
            //Debug.Log("Player entered the trigger zone!");
        }
        
    }
    public IEnumerator HideLockTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        lock_text.SetActive(false);
    }
}
