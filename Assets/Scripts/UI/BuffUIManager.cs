public class BuffUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        PickupInteractable.OnBuffActivated += AddIcon;
        PickupInteractable.OnBuffDeactivated += RemoveIcon;
    }

    protected override void UnsubscribeFromEvents()
    {
        PickupInteractable.OnBuffActivated -= AddIcon;
        PickupInteractable.OnBuffDeactivated -= RemoveIcon;
    }
}