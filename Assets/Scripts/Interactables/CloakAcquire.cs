using UnityEngine;

public class CloakAcquire : InteractableBase
{
    private PlayerState cloakAcquired;
    private void Awake()
    {
        cloakAcquired = new PlayerState()
        {
            hasCloak = true
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaiseAddSpell(SpellType.Cloak);
        GameEvents.RaisePlayerStateUpdate(cloakAcquired);
    }
}