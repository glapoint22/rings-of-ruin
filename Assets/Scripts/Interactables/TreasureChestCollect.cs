public class TreasureChestCollect : CollectibleInteractable
{
   private int coinCount; 
   protected override CollectibleType CollectibleType => CollectibleType.TreasureChest;

   public void SetCoinCount(int count)
   {
       coinCount = count;
   }
}