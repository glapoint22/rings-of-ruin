using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Segment Icon Library")]
public class SegmentIconLibrary : ScriptableObject
{
    [Header("Collectibles")]
    public Sprite gemIcon;
    public Sprite coinIcon;

    [Header("Hazards")]
    public Sprite spikeIcon;

    [Header("Pickups")]
    public Sprite shieldIcon;
    public Sprite cloakIcon;
    public Sprite timeDilationIcon;
    public Sprite healthIcon;
    public Sprite keyIcon;

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

    public Sprite GetPortalIcon(PortalType type)
    {
        return type switch
        {
            PortalType.PortalA => portalAIcon,
            PortalType.PortalB => portalBIcon,
            _ => null
        };
    }
} 