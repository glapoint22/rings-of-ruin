using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public virtual void Interact() {
        Destroy(gameObject);
    }
}