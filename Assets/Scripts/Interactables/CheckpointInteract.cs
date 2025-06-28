using UnityEngine;

public class CheckpointInteract : InteractableBase
{
    private Checkpoint checkpoint = new();
    protected override void OnInteract()
    {
        GameEvents.RaiseInteract(checkpoint, InteractableType.Checkpoint);
    }
}