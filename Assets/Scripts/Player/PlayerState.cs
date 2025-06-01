using UnityEngine;

/// <summary>
/// Holds the player’s state during gameplay — health, pickups, collected items, etc.
/// </summary>
public class PlayerState : MonoBehaviour
{
    private int gemsCollected = 0;

    public int GemsCollected => gemsCollected;

    /// <summary>
    /// Called when the player collects a gem in the level.
    /// </summary>
    public void AddGem()
    {
        gemsCollected++;
        Debug.Log($"[PlayerState] Gems Collected: {gemsCollected}");
        // TODO: Update UI, notify level progress tracker
    }

    /// <summary>
    /// Called when the player collects a coin.
    /// </summary>
    public void CollectCoin()
    {
        // We'll later move this to GameManager if coins are persistent
        Debug.Log("[PlayerState] Coin collected!");
        // TODO: Replace with GameManager.Instance.AddCoin(1);
    }

    public void ResetLevelStats()
    {
        gemsCollected = 0;
        // Reset other temporary states like shields, cloak, spell, etc. as we add them
    }
}