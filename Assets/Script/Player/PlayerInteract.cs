using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public List<BaseInteractable> interactables = new List<BaseInteractable> ();


    public void OnInteract()
    {
        if (interactables == null)
            return;

        float closestDistance = Mathf.Infinity;
        BaseInteractable closeIntercatable = null;

        foreach (var interactable in interactables)
        {
            float distance = Vector3.Distance(gameObject.transform.position, interactable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closeIntercatable = interactable;
            }
        }

        closeIntercatable.Exe();

        Debug.Log("플레이어 상호작용");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactable"))
        {
            BaseInteractable baseinter = collision.GetComponent<BaseInteractable> ();
            if (!interactables.Contains(baseinter))
            {
                interactables.Add(baseinter);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            BaseInteractable baseinter = collision.GetComponent<BaseInteractable>();
            if (interactables.Contains(baseinter))
            {
                interactables.Remove(baseinter);
            }

        }
    }
}
