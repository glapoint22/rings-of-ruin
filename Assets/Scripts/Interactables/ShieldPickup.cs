using UnityEngine;

public class ShieldPickup : InteractableBase
{
        protected override void OnInteract()
        {
                GameEvents.RaiseAddSpell(PickupType.Shield);
        }
}