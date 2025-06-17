using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract();
        }
    }

    protected abstract void OnInteract();
}