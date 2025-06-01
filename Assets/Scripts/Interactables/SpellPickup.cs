using UnityEngine;

public class SpellPickup : InteractableBase
{
    public override void Interact(PlayerState player)
    {
        Debug.Log("[SpellPickup] Spell acquired!");
        Destroy(gameObject);
    }
}
