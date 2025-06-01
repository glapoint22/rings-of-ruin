using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class PlayerInteractionHandler : MonoBehaviour
{
    private PlayerState playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();
        if (interactable != null)
        {
            interactable.Interact(playerState);
        }
    }
}
