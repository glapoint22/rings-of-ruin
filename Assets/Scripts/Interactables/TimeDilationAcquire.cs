using UnityEngine;

public class TimeDilationAcquire : InteractableBase
{
    private PlayerState timeDilationAcquired;
    private void Awake()
    {
        timeDilationAcquired = new PlayerState()
        {
            hasTimeDilation = true
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.TimeDilation);
        GameEvents.RaisePlayerStateUpdate(timeDilationAcquired);
    }
}