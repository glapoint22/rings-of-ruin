using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract();
            RaiseInteracted();
        }
    }


    protected virtual void RaiseInteracted()
    {
        GameEvents.RaiseInteracted(gameObject);
    }

    protected abstract void OnInteract();
}