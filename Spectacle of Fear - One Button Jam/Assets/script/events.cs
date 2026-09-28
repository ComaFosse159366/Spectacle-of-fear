using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class events : MonoBehaviour
{
    public bool won = false;
    public cutscene cutscene_manager;
    public bool sound_played_dog = false; // Flag to ensure the dog death sound is played only once
    public bool sound_played_mom = false; // Flag to ensure the mom death sound is played only once
    public bool sound_played_dad = false; // Flag to ensure the dad death sound is played only once
    public GameObject wintext;
    public inputcontroller input_controller;
    public GameObject player_group;
    [Header("music events")]
    public sound_and_music sound_manager;
    [Header("winning condition")]
    public bool beat_mom = false;
    public bool beat_dad = false;
    public bool beat_dog = false;
    public bool beat_dog_tughofwar = false;
    public bool beat_grandma = false;
    [Header("Life variables")]
    public float momlife = 100f;
    public float doglife = 100f;
    [Header("Event variables")]
    public GameObject event_object;
    public int event_type = 0; //0 = mom, 1 = dad, 2 = dog, 3 = grandma
    public GameObject momevent;
    public GameObject dadevent;
    public GameObject dogevent;
    public GameObject grandmaevent;
    [Header("Rythm variables")]
    public int distance = 6;
    private int mouvementspeed = 15;
    public GameObject hitpoint;
    public float distanceToTarget;
    public float damage;
    public Collider rythm_collider;
    public GameObject hitpoint_target;
    public SpriteRenderer hitpoint_renderer;
    public SpriteRenderer mom_renderer;
    public Sprite mom_scared;
    public Sprite mom_stabbed;
    public Sprite hitpoint_black;
    public Sprite hitpoint_white;
    public GameObject Spawnpoint;
    public Collider other_collider;
    private Vector3 lastOtherPosition;
    private bool hasLastOtherPosition = false;
    private bool iscolliding = false;
    [Header("Dad click variables")]
    public Slider dadslidebar;
    public Slider playerslidebar;
    public int dadclick = 1;
    private int dadclickvalue = 50;// each start with 50
    public int playerclick = 1;
    private int playerclickvalue = 50;//each start with 50
    private int toatalclickvalue = 100;// total must be 100 and when player go up dad go down and vice versa
    private int dadclicktime = 100; // each second dad click
    [Header("generalvariables")]
    public bool isinteracting = false;
    [SerializeField] private string hitpointTag = "hitpoint";
    [Header("MoMevent")]
    public Slider momlifebar;
    private int randominterval = 500; //initial interval for spawning hitpoints
    [Header("grandma event")]
    public float holdtime = 15f; // time of holding the button to win the grandma event
    public bool isholding = false; // check if the player is holding the button
    [Header("Dog event")]
    public Slider dogslidebar;
    public Slider player_dogslidebar;
    public int dogclick = 1;
    private int dogclickvalue = 50;// each start with 50
    public int playerclick_dog = 1;
    private int player_dogclickvalue = 50;//each start with 50
    private int dogtoatalclickvalue = 100;// total must be 100 and when player go up dad go down and vice versa
    private int dogclicktime = 100; // each second dad click
    public Slider doglifebar;
    public GameObject Spawnpoint_dog;
    public SpriteRenderer dog_renderer;
    public Sprite dog_scared;
    public Sprite dog_stabbed;
    public GameObject dog_tugh_of_war;
    public GameObject dog_rythm_group;
    [Header("event holder")]
    public GameObject event_holder1;
    public GameObject event_holder2;
    public GameObject event_holder3;
    public GameObject event_holder4;
    public int event_id;

    public void Update()
    {
        switch (event_type)
        {
            case 0: //mom event
                 //every 3seconde spawn a hitpoint
                if (Time.frameCount % randominterval == 0)
                {
                    SpawnHitpoint();
                }
                if  (Time.frameCount % 700 == 0)
                {
                    randominterval = Random.Range(350, 650);
                }
                momlifebar.value = momlife;
                if (momlife <= 0)
                {
                    beat_mom = true;
                    end_mom_event();
                    if(!sound_played_mom)
                    {
                        sound_played_mom = true;
                        sound_manager.PlaySound(sound_manager.mom_death_sound);
                    }
                }
                break;
            case 1://dad event
                //every 1 second do dad click event and the time reduce when he start losing
                if (Time.frameCount % dadclicktime == 0)
                {
                    dadclickvalue += dadclick;
                if (dadclick > 60 | dadclick < 100)
                {
                dadclicktime = Mathf.RoundToInt(100 - (dadclickvalue/2) ); //reduce the time when dad click value is low
                }
                dadslidebar.value = dadclickvalue;
                playerclickvalue -= dadclick;
                playerslidebar.value = playerclickvalue;
                }
                if(playerclickvalue == 100)
                {
                    beat_dad = true;
                    end_dad_event();
                    if(!sound_played_dad)
                    {
                        sound_played_dad = true;
                        sound_manager.PlaySound(sound_manager.dad_death_sound);
                    }
                }
                break;
            case 3: //dog event
                //every 1 second do dad click event and the time reduce when he start losing
                if (!beat_dog_tughofwar)
                {
                    if (Time.frameCount % dogclicktime == 0)
                    {
                        dogclickvalue += dogclick;
                    if (dogclick > 60 | dogclick < 100)
                    {
                    dogclicktime = Mathf.RoundToInt(100 - (dogclickvalue/2) ); //reduce the time when dad click value is low
                    }
                    dogslidebar.value = dogclickvalue;
                    player_dogclickvalue -= dogclick;
                    player_dogslidebar.value = player_dogclickvalue;
                    }
                    if(player_dogclickvalue == 100)
                    {
                        beat_dog_tughofwar = true;
                        dog_rythm();
                    }
                }  
                else
                 //every 3seconde spawn a hitpoint
                if (Time.frameCount % randominterval == 0)
                {
                    SpawnHitpoint_dog();
                }
                if  (Time.frameCount % 700 == 0)
                {
                    randominterval = Random.Range(350, 650);
                }
                doglifebar.value = doglife;
                if (doglife <= 0)
                {
                    beat_dog = true;
                    end_dog_event();
                    if(!sound_played_dog)
                    {
                        sound_played_dog = true;
                        sound_manager.PlaySound(sound_manager.dog_death_sound);
                    }
                    
                }
                break;
            case 2: //grandma event
                if (isholding & !beat_grandma)
                {
                    holdtime -= Time.deltaTime;
                    if (holdtime <= 0f)
                    {
                        beat_grandma = true;
                        Debug.Log("Grandma death!");
                        end_grandma_event();
                    }
                }
                break;
            case 4:
            // nothing 
                break;
            default:
                Debug.LogWarning("Invalid event_type: " + event_type);
                break;
        }
        if (beat_mom && beat_dad && beat_dog && beat_grandma && !won)
        {
            won = true;
            Debug.Log("All events completed! You win!");
            // Implement the logic for winning the game here
            wintext.SetActive(true);
            cutscene_manager.start_cutscene(); // Start the cutscene when all events are completed
            
        }
    }
    void Start()
    {
        wintext.SetActive(false);
        if (rythm_collider.gameObject != gameObject)
        {
            TriggerRelay relay = rythm_collider.GetComponent<TriggerRelay>();
            if (relay == null)
            {
                relay = rythm_collider.gameObject.AddComponent<TriggerRelay>();
            }
            relay.owner = this;
        }
        momlifebar.value = momlife;
        rythm_collider.enabled = true;
        hitpoint_renderer.sprite = hitpoint_black;
        hitpoint_renderer.enabled = false;
        playerslidebar.value = 50;
        dadslidebar.value = 50;
        int[] eventIds = { 0, 1, 2, 3 };
        for (int i = 0; i < eventIds.Length; i++)
        {
            int j = Random.Range(i, eventIds.Length);
            int temp = eventIds[i];
            eventIds[i] = eventIds[j];
            eventIds[j] = temp;
        }
        event_holder1.GetComponent<event_executer>().event_id = eventIds[0];
        event_holder2.GetComponent<event_executer>().event_id = eventIds[1];
        event_holder3.GetComponent<event_executer>().event_id = eventIds[2];
        event_holder4.GetComponent<event_executer>().event_id = eventIds[3];
    }
    // ==========================================
    // rythm mom game
    // ==========================================
    #region rythm mom game
    [ContextMenu("Start Mom Event")]
    public void start_mom_event()
    {
        event_type = 0;
        momevent.SetActive(true);
        hitpoint_renderer.enabled = true;
        player_group.SetActive(false);
        event_object.transform.position = new Vector3(-16, 32, 0);
        input_controller.gamestate = 1; // Return to wonder state
    }
    public void end_mom_event()
    {
        // Implement the logic to end the mom event here
        momevent.SetActive(false);
        player_group.SetActive(true);
        hitpoint_renderer.enabled = false;
        input_controller.gamestate = 0; // Return to wonder state
        event_type = 4; // Reset event type to default
    }
    private sealed class TriggerRelay : MonoBehaviour
    {
        public events owner;

        private void OnTriggerStay(Collider other)
        {
            if (owner != null)
            {
                owner.HandleTriggerStay(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (owner != null)
            {
                owner.HandleTriggerExit(other);
            }
        }
    }

    
    
    public void rytm_click()
    {
        StartCoroutine(rythm_click());
    }
    public IEnumerator rythm_click()
    {
        if (!iscolliding)
        {
            yield break; // Exit if already interacting
        }
        Debug.Log("Rythm click started");
        isinteracting = true;
        mom_renderer.sprite = mom_stabbed;
        hitpoint_renderer.sprite = hitpoint_white;
        sound_manager.PlaySound(sound_manager.attack_sound);
        calculate_rythm_value(other_collider);
        yield return new WaitForSeconds(0.1f);
        hitpoint_renderer.sprite = hitpoint_black;
        mom_renderer.sprite = mom_scared;
        isinteracting = false;
    }
    public void calculate_rythm_value(Collider other)
    {
        //calculate the score based on the distance between the hitpoint and the target
        Vector3 otherPosition;

        if (other != null)
        {
            otherPosition = other.transform.position;
            lastOtherPosition = otherPosition;
            hasLastOtherPosition = true;
        }
        else if (hasLastOtherPosition)
        {
            otherPosition = lastOtherPosition;
        }
        else
        {
            Debug.LogWarning("events: No valid collider available to calculate rhythm value.");
            return;
        }

        distanceToTarget = Vector3.Distance(hitpoint.transform.position, otherPosition);
        damage = 10 - distanceToTarget * 2.5f;
        Debug.Log("Damage: " + damage);
        damage = Mathf.Round(damage * 10f) / 10f; // Round to one decimal place
        momlife -= damage;
    }
    private void OnTriggerStay(Collider other)
    {
        HandleTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        HandleTriggerExit(other);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag(hitpointTag))
        {
            lastOtherPosition = other.transform.position;
            hasLastOtherPosition = true;
        }

        HandleTriggerStay(other);
        other_collider = other;
        iscolliding = true;
    }

    public void HandleTriggerStay(Collider other)
    {
        if (other != null && other.CompareTag(hitpointTag))
        {
            lastOtherPosition = other.transform.position;
            hasLastOtherPosition = true;
        }

        if(isinteracting)
        {
        hitpoint_renderer.sprite = hitpoint_white;
        Debug.Log("Hitpoint hit!");

        //calculate the distance between the hitpoint and the target
        distanceToTarget = Vector3.Distance(hitpoint.transform.position, other.transform.position);
        GameObject objectToDestroy = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
        Destroy(objectToDestroy);
        if (other == other_collider)
        {
            other_collider = null;
        }
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (other == null || !other.CompareTag(hitpointTag))
        {
            return;
        }

        if (other == other_collider)
        {
            other_collider = null;
        }

        Debug.Log("Hitpoint quitted!");
        hitpoint_renderer.sprite = hitpoint_black;
    }
    [ContextMenu("Spawn Hitpoint")]
    public void SpawnHitpoint()
    {
        Vector3 spawnpoint = Spawnpoint.transform.position;
        Vector3 target = GetXAxisTarget(spawnpoint);
        StartCoroutine(spawn_hitpoint(target, spawnpoint));
    }
    public IEnumerator spawn_hitpoint(Vector3 Target, Vector3 spawnpoint)
    {
        GameObject spawnedHitpoint = Instantiate(hitpoint, spawnpoint, Quaternion.identity);
        //spawnedHitpoint.transform.LookAt(Target);
        StartCoroutine(SlideTo(spawnedHitpoint, Target));
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator SlideTo(GameObject spawnedHitpoint, Vector3 targetWorld)
    {
        float speed = mouvementspeed;
        if (spawnedHitpoint == null)
        {
            yield break;
        }

        float fixedY = spawnedHitpoint.transform.position.y;
        float fixedZ = spawnedHitpoint.transform.position.z;

        while (spawnedHitpoint != null && (spawnedHitpoint.transform.position - targetWorld).sqrMagnitude > 0.0001f)
        {
            Vector3 next = Vector3.MoveTowards(spawnedHitpoint.transform.position, targetWorld, speed * Time.deltaTime);
            next.y = fixedY;
            next.z = fixedZ;
            spawnedHitpoint.transform.position = next;
            yield return null;
        }

        // Snap to exact X destination while preserving the locked axes.
        if (spawnedHitpoint != null)
        {
            spawnedHitpoint.transform.position = new Vector3(targetWorld.x, fixedY, fixedZ);
        }
    }

    private Vector3 GetXAxisTarget(Vector3 spawnpoint)
    {
        if (hitpoint_target == null)
        {
            Debug.LogWarning("events: hitpoint_target is not assigned. Using +X direction.");
            return new Vector3(spawnpoint.x + Mathf.Abs(distance), spawnpoint.y, spawnpoint.z);
        }

        float xSign = Mathf.Sign(hitpoint_target.transform.position.x - spawnpoint.x);
        if (Mathf.Approximately(xSign, 0f))
        {
            xSign = 1f;
        }

        return new Vector3(spawnpoint.x + xSign * Mathf.Abs(distance), spawnpoint.y, spawnpoint.z);
    }
    #endregion
    // ==========================================
    // clicking dad game
    // ==========================================
    #region clicking dad game
    public void playerclicks()
    {
        playerclickvalue += playerclick;
        dadclickvalue -= playerclick;
        if (playerclickvalue >= toatalclickvalue)
        {
            dadclickvalue = 0;
        }
        dadslidebar.value = dadclickvalue;
        playerslidebar.value = playerclickvalue;
    }
    public void start_dad_event()
    {
        event_type = 1;
        hitpoint_renderer.enabled = true;
        dadevent.SetActive(true);
        player_group.SetActive(false);
        input_controller.gamestate = 1; // Set to encounter state
    }
    public void end_dad_event()
    {
        // Implement the logic to end the dad event here
        dadevent.SetActive(false);
        hitpoint_renderer.enabled = false;
        player_group.SetActive(true);
        input_controller.gamestate = 0; // Return to wonder state
        event_type = 4; // Reset event type to default
    }
    #endregion
     // =========================================
    // clicking grandma game
    // ==========================================
    #region clicking grandma game
    public void start_hold()
    {
        sound_manager.PlaySound(sound_manager.attack_sound);
        isholding = true;
        Debug.Log("Long click started for grandma event");
    }
    public void stop_hold()
    {
        isholding = false;
        Debug.Log("Long click stopped for grandma event");
    }
    public void start_grandma_event()
    {
        event_type = 2;
        hitpoint_renderer.enabled = true;
        grandmaevent.SetActive(true);
        player_group.SetActive(false);
        input_controller.gamestate = 1; // Set to encounter state
    }
    public void end_grandma_event()
    {
        // Implement the logic to end the grandma event here
        hitpoint_renderer.enabled = false;
        grandmaevent.SetActive(false);
        player_group.SetActive(true);
        input_controller.gamestate = 0; // Return to wonder state
        event_type = 4; // Reset event type to default
    }
    #endregion
     // =========================================
    // clicking dog game
    // ==========================================
    #region dog game
    [ContextMenu("Start Dog Event")]
    public void start_dog_event()
    {
        event_type = 3;
        dogevent.SetActive(true);
        player_group.SetActive(false);
        dog_tugh_of_war.SetActive(true);
        dog_rythm_group.SetActive(false);
        hitpoint_renderer.enabled = true;
        event_object.transform.position = new Vector3(24, 110, 0); // 126
        input_controller.gamestate = 1; // Set to encounter state
    }
    public void dog_combat()
    {
        // Implement the logic for the dog combat game here
        //initiale everything 

        //start with the tugh of war game and then go to the rythm game
        dog_tugofwar();
        if(beat_dog_tughofwar)
        {
            dog_rythm();
        }
    }
    public void end_dog_event()
    {
        // Implement the logic to end the dog tug of war event here
        dog_tugh_of_war.SetActive(false);
        dog_rythm_group.SetActive(false);
        player_group.SetActive(true);
        hitpoint_renderer.enabled = false;
        input_controller.gamestate = 0; // Return to wonder state
        event_type = 4; // Reset event type to default
    }
    #endregion
    #region Rytm dog
    public void dog_rythm()
    {
        // Implement the logic for the dog rhythm game here
        event_object.transform.position = new Vector3(126, 24, 0); // 126
        dog_tugh_of_war.SetActive(false);
        dog_rythm_group.SetActive(true);
    }
    public void SpawnHitpoint_dog()
    {
        Vector3 spawnpoint = Spawnpoint_dog.transform.position;
        Vector3 target = GetXAxisTarget(spawnpoint);
        StartCoroutine(spawn_hitpoint(target, spawnpoint));
    }
    public void rytm_click_dog()
    {
        StartCoroutine(rythm_click_dog());
        Debug.Log("simple click dog");
    }
    public IEnumerator rythm_click_dog()
    {
        if (!iscolliding)
        {
            yield break; // Exit if already interacting
        }
        Debug.Log("Rythm click started");
        isinteracting = true;
        dog_renderer.sprite = dog_stabbed;
        hitpoint_renderer.sprite = hitpoint_white;
        sound_manager.PlaySound(sound_manager.attack_sound);
        calculate_rythm_value_dog(other_collider);
        yield return new WaitForSeconds(0.1f);
        hitpoint_renderer.sprite = hitpoint_black;
        dog_renderer.sprite = dog_scared;
        isinteracting = false;
    }
    public void calculate_rythm_value_dog(Collider other)
    {
        //calculate the score based on the distance between the hitpoint and the target
        Vector3 otherPosition;

        if (other != null)
        {
            otherPosition = other.transform.position;
            lastOtherPosition = otherPosition;
            hasLastOtherPosition = true;
        }
        else if (hasLastOtherPosition)
        {
            otherPosition = lastOtherPosition;
        }
        else
        {
            Debug.LogWarning("events: No valid collider available to calculate rhythm value.");
            return;
        }

        distanceToTarget = Vector3.Distance(hitpoint_target.transform.position, otherPosition);
        damage = 10 - distanceToTarget * 2.5f;
        Debug.Log("Damage: " + damage);
        damage = Mathf.Round(damage * 10f) / 10f; // Round to one decimal place
        doglife -= damage;
    }
    #endregion
    #region tugh of war dog
    public void dog_tugofwar()
    {
        // Implement the logic for the dog tug of war game here
    }
    public void playerclicks_dog()
    {
        player_dogclickvalue += playerclick;
        dogclickvalue -= playerclick;
        if (player_dogclickvalue >= dogtoatalclickvalue)
        {
            dogclickvalue = 0;
        }
        dogslidebar.value = dogclickvalue;
        player_dogslidebar.value = player_dogclickvalue;
    }
    
    #endregion
}
