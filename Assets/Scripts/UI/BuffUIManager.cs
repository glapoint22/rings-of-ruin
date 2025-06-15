public class BuffUIManager : BaseUIManager
{
    protected override void SubscribeToEvents()
    {
        InteractableManager.OnBuffActivated += AddIcon;
        InteractableManager.OnBuffDeactivated += RemoveIcon;
    }

    protected override void UnsubscribeFromEvents()
    {
        InteractableManager.OnBuffActivated -= AddIcon;
        InteractableManager.OnBuffDeactivated -= RemoveIcon;
    }
}