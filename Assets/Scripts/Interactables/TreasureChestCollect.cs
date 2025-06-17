public class TreasureChestCollect : InteractableBase
{
   private int coinCount; 

   public void SetCoinCount(int count)
   {
       coinCount = count;
   }

   protected override void OnInteract() {
    GameEvents.RaiseTreasureChestCollect(coinCount);
   }
}