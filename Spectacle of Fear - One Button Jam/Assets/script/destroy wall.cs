using UnityEngine;

public class destroywall : MonoBehaviour
{
    public events eventsScript; // Reference to the events script

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || !IsHitpoint(other))
        {
            return;
        }

        GameObject objectToDestroy = GetHitpointRoot(other);
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }

        if (eventsScript != null)
        {
            if(eventsScript.event_type == 1)
            {
            eventsScript.momlife = eventsScript.momlife + 10f; // Increase momlife by 10
            }
            else if(eventsScript.event_type == 2)
            {
            eventsScript.doglife = eventsScript.doglife + 10f; // Increase doglife by 10
            }
        }
    }

    private bool IsHitpoint(Collider other)
    {
        if (other.CompareTag("hitpoint"))
        {
            return true;
        }

        Transform current = other.transform;
        while (current != null)
        {
            if (current.CompareTag("hitpoint"))
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    private GameObject GetHitpointRoot(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            return other.attachedRigidbody.gameObject;
        }

        Transform current = other.transform;
        while (current.parent != null)
        {
            current = current.parent;
        }

        return current.gameObject;
    }
}
