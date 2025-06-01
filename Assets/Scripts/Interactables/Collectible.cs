using UnityEngine;

public class Collectible : InteractableBase
{
    [SerializeField] private CollectibleType type;

    public override void Interact(PlayerState player)
    {
        switch (type)
        {
            case CollectibleType.Gem:
                player.AddGem();
                break;

            case CollectibleType.Coin:
                player.CollectCoin(); // We’ll move this to GameManager later
                break;

            default:
                Debug.LogWarning($"Unhandled collectible type: {type}");
                break;
        }

        // TODO: Play VFX/SFX
        Destroy(gameObject); // For now, just remove the collectible
    }

    public CollectibleType GetCollectibleType()
    {
        return type;
    }
}
