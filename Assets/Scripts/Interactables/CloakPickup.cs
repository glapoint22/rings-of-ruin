using UnityEngine;

public class CloakPickup : InteractableBase
{
    private PlayerState cloakUpdate;
    private void Awake()
    {
        cloakUpdate = new PlayerState()
        {
            hasCloak = true
        };
    }

    protected override void OnInteract()
    {
        GameEvents.RaisePlayerStateUpdate(cloakUpdate);
        GameEvents.RaiseAddSpell(PickupType.Cloak);
    }
}