using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Segment Icon Library")]
public class SegmentIconLibrary : ScriptableObject
{
    [Header("Segment Types")]
    public Sprite gapIcon;
    public Sprite crumblingIcon;
    public Sprite spikeIcon;


    [Header("Collectibles")]
    public Sprite gemIcon;
    public Sprite coinIcon;
    public Sprite treasureChestIcon;


    [Header("Spells")]
    public Sprite shieldIcon;
    public Sprite cloakIcon;
    public Sprite timeDilationIcon;
    public Sprite stormboltIcon;
    public Sprite bloodrootIcon;
    public Sprite fireballIcon;
    public Sprite ashbindIcon;
    

    [Header("Enemies")]
    public Sprite ruinwalkerIcon;
    public Sprite gravecallerIcon;
    public Sprite bloodseekerIcon;


    [Header("Portals")]
    public Sprite portalAIcon;
    public Sprite portalBIcon;



    [Header("Utility Items")]
    public Sprite keyIcon;
    public Sprite playerIcon;
    public Sprite bridgeIcon;
    public Sprite healthIcon;
    


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


    public Sprite GetSpellIcon(SpellType type)
    {
        return type switch
        {
            SpellType.Shield => shieldIcon,
            SpellType.Cloak => cloakIcon,
            SpellType.TimeDilation => timeDilationIcon,
            SpellType.Stormbolt => stormboltIcon,
            SpellType.Bloodroot => bloodrootIcon,
            SpellType.Fireball => fireballIcon,
            SpellType.Ashbind => ashbindIcon,
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

     public Sprite GetUtilityItemIcon(UtilityItem type)
    {
        return type switch
        {
            UtilityItem.Key => keyIcon,
            UtilityItem.Player => playerIcon,
            UtilityItem.Bridge => bridgeIcon,
            UtilityItem.Health => healthIcon,
            _ => null
        };
    }
} 