public class StormboltPickup : PickupInteractable
{
   protected override PickupType PickupType => PickupType.Stormbolt;

   protected override void OnInteract()
   {
       Remove();
   }
}