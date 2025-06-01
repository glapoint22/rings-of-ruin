using UnityEngine;

/// <summary>
/// Base class for any object the player can interact with (e.g., collectibles, pickups).
/// </summary>
public abstract class InteractableBase : MonoBehaviour
{
    /// <summary>
    /// Called when the player interacts with this object.
    /// </summary>
    public abstract void Interact(PlayerState player);
}