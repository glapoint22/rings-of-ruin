using System;

public abstract class PickupInteractable : InteractableBase
{
    protected abstract PickupType PickupType { get; }
}