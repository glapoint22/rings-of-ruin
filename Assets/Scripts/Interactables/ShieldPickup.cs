public class ShieldPickup : InteractableBase
{
        protected override void OnInteract()
        {
            GameEvents.RaiseShieldPickup();
        }
}