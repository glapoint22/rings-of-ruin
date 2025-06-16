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


    protected void Remove() {
        Destroy(gameObject);
    }
    

    protected abstract void OnInteract();
}