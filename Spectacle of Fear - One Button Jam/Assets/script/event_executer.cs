using UnityEngine;

public class event_executer : MonoBehaviour
{
    public actionmanager action_manager;
    public sound_and_music sound_manager;
    public int event_id;
    public events event_manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Update()
    {
        switch (event_id)
        {
            case 0:
                if (event_manager.beat_mom)
                {
                    Debug.Log("Event mom executed");
                    sound_manager.StopMusic();
                    sound_manager.StartMusic(sound_manager.musicClip);
                    Destroy(gameObject); // Destroy the event_executer object after executing the event
                }
                break;
            case 1:
                if (event_manager.beat_dad)
                {
                    Debug.Log("Event dad executed");
                    sound_manager.StopMusic();
                    sound_manager.StartMusic(sound_manager.musicClip);
                    Destroy(gameObject); // Destroy the event_executer object after executing the event
                }
                break;
            case 2:
                if (event_manager.beat_dog)
                {
                    Debug.Log("Event dog executed");
                    sound_manager.StopMusic();
                    sound_manager.StartMusic(sound_manager.musicClip);
                    Destroy(gameObject); // Destroy the event_executer object after executing the event
                }
                break;
            case 3:
                if (event_manager.beat_grandma)
                {
                    Debug.Log("Event grandma executed");
                    sound_manager.StopMusic();
                    sound_manager.StartMusic(sound_manager.musicClip); // Start the music when grandma event is executed
                    Destroy(gameObject); // Destroy the event_executer object after executing the event
                }
                break;
            default:
                Debug.Log("Invalid event ID");
                break;
        }
    }
    public void execute_event()
    {
        switch (event_id)
        {
            case 0:
                Debug.Log("Event mom executed");
                    event_manager.start_mom_event();
                break;
            case 1:
                Debug.Log("Event dad executed");
                    event_manager.start_dad_event();
                break;
            case 2:
                Debug.Log("Event dog executed");
                    event_manager.start_dog_event();
                break;
            case 3:
                Debug.Log("Event grandma executed");
                    event_manager.start_grandma_event();
                break;
            default:
                Debug.Log("Invalid event ID");
                break;
        }
        sound_manager.StopMusic();
        sound_manager.PlaySound(sound_manager.fight_music);
    }
    private void OnTriggerStay()
    {
        if (action_manager.isinteracting)
        {
            execute_event();
        }
    }
}
