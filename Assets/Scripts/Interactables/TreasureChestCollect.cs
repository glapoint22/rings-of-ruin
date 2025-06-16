public class TreasureChestCollect : CollectibleInteractable
{
   private int coinCount; 

   public void SetCoinCount(int count)
   {
       coinCount = count;
   }

   protected override CollectibleType CollectibleType => CollectibleType.TreasureChest;

   protected override void OnInteract()
   {
       Remove();
   }
}