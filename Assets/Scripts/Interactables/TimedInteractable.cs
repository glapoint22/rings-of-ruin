using UnityEngine;
using System.Collections;

public abstract class TimedInteractable : BuffPickupInteractable
{
    [SerializeField] protected float duration = 10f;
    
    protected IEnumerator StartTimer()
    {
        // Disable visual and physics components but keep the GameObject active
        DisableVisuals();
        yield return new WaitForSeconds(duration);
        DeactivateBuff();
        Remove();
    }


    protected override void OnInteract()
    {
        ActivateBuff();
        StartCoroutine(StartTimer());
    }

    protected virtual void DisableVisuals()
    {
        // Disable renderer
        if (TryGetComponent<Renderer>(out var renderer))
            renderer.enabled = false;
            
        // Disable collider
        if (TryGetComponent<Collider>(out var collider))
            collider.enabled = false;
    }
}