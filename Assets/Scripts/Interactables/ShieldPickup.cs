using UnityEngine;

public class ShieldPickup : InteractableBase
{
        [SerializeField] private int shieldHealth;
        private Shield shield;

        private void Awake()
        {
                shield = new Shield(shieldHealth);
        }

        protected override void OnInteract()
        {
                GameEvents.RaisePickup(shield, PickupType.Shield);
        }
}