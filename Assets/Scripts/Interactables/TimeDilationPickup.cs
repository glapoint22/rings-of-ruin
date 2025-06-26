using UnityEngine;

public class TimeDilationPickup : InteractableBase
{
   [SerializeField] private float duration;
    private TimeDilation timeDilation;
    private void Awake()
    {
        timeDilation = new TimeDilation(duration);
    }
    protected override void OnInteract()
    {
        GameEvents.RaisePickup(timeDilation, PickupType.TimeDilation);
    }
}