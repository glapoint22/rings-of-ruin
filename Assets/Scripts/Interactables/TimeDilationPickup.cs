using UnityEngine;

public class TimeDilationPickup : InteractableBase
{
    private PlayerState timeDilationUpdate;
    private void Awake()
    {
        timeDilationUpdate = new PlayerState()
        {
            hasTimeDilation = true
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(timeDilationUpdate);
        GameEvents.RaiseAddSpell(PickupType.TimeDilation);
    }
}