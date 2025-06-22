using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Segment Icon Library")]
public class SegmentIconLibrary : ScriptableObject
{
    [Header("Collectibles")]
    public Sprite gemIcon;
    public Sprite coinIcon;
    public Sprite treasureChestIcon;

    [Header("Segment Types")]
    public Sprite gapIcon;
    public Sprite crumblingIcon;
    public Sprite spikeIcon;

    [Header("Pickups")]
    public Sprite shieldIcon;
    public Sprite cloakIcon;
    public Sprite timeDilationIcon;
    public Sprite healthIcon;
    public Sprite keyIcon;
    public Sprite stormboltIcon;
    public Sprite bloodrootIcon;
    public Sprite fireballIcon;
    

    [Header("Enemies")]
    public Sprite ruinwalkerIcon;
    public Sprite gravecallerIcon;
    public Sprite bloodseekerIcon;

    [Header("Portals")]
    public Sprite portalAIcon;
    public Sprite portalBIcon;

    [Header("Checkpoint")]
    public Sprite checkpointIcon;

    public Sprite GetCollectibleIcon(CollectibleType type)
    {
        return type switch
        {
            CollectibleType.Gem => gemIcon,
            CollectibleType.Coin => coinIcon,
            CollectibleType.TreasureChest => treasureChestIcon,
            _ => null
        };
    }


    public Sprite GetPickupIcon(PickupType type)
    {
        return type switch
        {
            PickupType.Shield => shieldIcon,
            PickupType.Cloak => cloakIcon,
            PickupType.TimeDilation => timeDilationIcon,
            PickupType.Health => healthIcon,
            PickupType.Key => keyIcon,
            PickupType.Stormbolt => stormboltIcon,
            PickupType.Bloodroot => bloodrootIcon,
            PickupType.Fireball => fireballIcon,
            _ => null
        };
    }

    public Sprite GetEnemyIcon(EnemyType type)
    {
        return type switch
        {
            EnemyType.Ruinwalker => ruinwalkerIcon,
            EnemyType.Gravecaller => gravecallerIcon,
            EnemyType.Bloodseeker => bloodseekerIcon,
            _ => null
        };
    }

    public Sprite GetInteractableIcon(InteractableType type)
    {
        return type switch
        {
            InteractableType.PortalA => portalAIcon,
            InteractableType.PortalB => portalBIcon,
            InteractableType.Checkpoint => checkpointIcon,
            _ => null
        };
    }

    public Sprite GetSegmentTypeIcon(SegmentType type)
    {
        return type switch
        {
            SegmentType.Gap => gapIcon,
            SegmentType.Crumbling => crumblingIcon,
            SegmentType.Spike => spikeIcon,
            _ => null
        };
    }
} 