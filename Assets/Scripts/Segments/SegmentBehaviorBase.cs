using UnityEngine;

/// <summary>
/// Base class for segment behaviors like gaps or crumbling tiles.
/// </summary>
public abstract class SegmentBehaviorBase : MonoBehaviour
{
    public abstract void OnPlayerStep(PlayerState player);
}