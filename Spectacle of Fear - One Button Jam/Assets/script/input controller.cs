using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class inputcontroller : MonoBehaviour
{
    public int gamestate = 0; //0 = wonder 1= encounter 2 = cutscene
    public cutscene cutscene_manager;
    public actionmanager action_manager;
    [SerializeField] private InputActionAsset inputActions;
    private InputAction buttonAction;
    public events events;

    private void Awake()
    {
        var playerMap = inputActions.FindActionMap("Player", true);
        buttonAction = playerMap.FindAction("button", true);
    }
    [ContextMenu("Toggle Game State")]
    private void toggleGameState()
    {
        gamestate = (gamestate == 0) ? 1 : 0;
    }
    private void OnEnable()
    {
        buttonAction.performed += OnButtonPerformed;
        buttonAction.canceled += OnButtonCanceled;
        buttonAction.Enable();
    }

    private void OnDisable()
    {
        buttonAction.performed -= OnButtonPerformed;
        buttonAction.canceled -= OnButtonCanceled;
        buttonAction.Disable();
    }

    private void OnButtonPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            simpleclick();
        }
        else if (context.interaction is MultiTapInteraction)
        {
            doubleclick();
        }
        else if (context.interaction is HoldInteraction)
        {
            longclick();
        }
    }

    private void OnButtonCanceled(InputAction.CallbackContext context)
    {
        releaseclick();
    }
    [ContextMenu("Simple Click")]
    public void simpleclick()
    {
        switch (gamestate)
        {
            case 0:
            // Handle simple click in wonder state
                action_manager.rotate();
                break;
            case 1:
            // Handle simple click in encounter state if needed
                switch (events.event_type)
                {
                    case 0:
                        events.rytm_click();
                        break;
                    case 1:
                    // Handle simple click in encounter state for event type 1 if needed
                        //events.playerclicks();
                        break;
                    case 3:
                    // Handle simple click in encounter state for event type 2 if needed
                    if (events.beat_dog_tughofwar)
                        {
                            events.rytm_click_dog();
                        }
                        break;
                    default:
                    // Handle simple click in encounter state for unknown event type if needed
                        break;
                }
                break;
            case 2:
            // Handle simple click in cutscene state if needed
                cutscene_manager.next_frame();
                break;
            default:
            // Handle simple click in unknown state if needed
                break;
        }  
    }

    public void doubleclick()
    {
        switch (gamestate)
        {
            case 0:
                action_manager.move();
                break;
            case 1:
                switch (events.event_type)
                {
                    case 0:
                        events.rytm_click();
                        break;
                    case 1:
                    // Handle simple click in encounter state for event type 1 if needed
                        events.playerclicks();
                        break;
                    case 3:
                    // Handle simple click in encounter state for event type 2 if needed
                    if (!events.beat_dog_tughofwar)
                        {
                            events.playerclicks_dog();
                        }
                        break;
                    default:
                    // Handle simple click in encounter state for unknown event type if needed
                        break;
                }
            // Handle double click in encounter state if needed
                break;
            default:
            // Handle double click in unknown state if needed
                break;
        } 
    }

    public void longclick()
    {
        switch (gamestate)
        {
            case 0:
            // Handle long click in wonder state if needed
            action_manager.interact();
                break;
            case 1:
            switch (events.event_type)
                {
                    case 0:
                        break;
                    case 1:
                    // Handle long click in encounter state for event type 1 if needed
                        break;
                    // Handle long click in encounter state for event type 1 if needed
                    case 2:
                    // Handle long click in encounter state for event type 2 if needed
                        events.start_hold();
                        Debug.Log("Long click started for event type 2");
                        break;
                    default:
                    // Handle long click in encounter state for unknown event type if needed
                        break;
                }
            // Handle long click in encounter state if needed
                break;
            default:
            // Handle long click in unknown state if needed
                break;
        }
    }

    public void releaseclick()
    {
        switch (gamestate)
        {
            case 0:
            // Handle release click in wonder state if needed
            action_manager.stopinteract();
                break;
            case 1:
            // Handle release click in encounter state if needed
                switch (events.event_type)
                {
                    case 0:
                        break;
                    case 1:
                    // Handle release click in encounter state for event type 1 if needed
                        break;
                    case 2:
                    // Handle release click in encounter state for event type 2 if needed
                        events.stop_hold();
                        break;
                    default:
                    // Handle release click in encounter state for unknown event type if needed
                        break;
                }
                break;
            default:
            // Handle release click in unknown state if needed
                break;
        }
    }
}
