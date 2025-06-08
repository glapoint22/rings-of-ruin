using System;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }
}
