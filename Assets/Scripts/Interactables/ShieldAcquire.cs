using UnityEngine;

public class ShieldAcquire : InteractableBase
{
        protected override void OnInteract()
        {
                GameEvents.RaiseAddSpell(SpellType.Shield);
        }
}