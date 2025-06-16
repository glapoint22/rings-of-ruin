public class BuffUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        BuffPickupInteractable.OnBuffActivated += AddIcon;
        BuffPickupInteractable.OnBuffDeactivated += RemoveIcon;
    }

    protected override void UnsubscribeFromEvents()
    {
        BuffPickupInteractable.OnBuffActivated -= AddIcon;
        BuffPickupInteractable.OnBuffDeactivated -= RemoveIcon;
    }
}