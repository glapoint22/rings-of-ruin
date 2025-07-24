using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Segment Icon Library")]
public class SegmentIconLibrary : ScriptableObject
{
    [Header("Segment Types")]
    public Sprite gapIcon;
    public Sprite crumblingIcon;
    public Sprite spikeIcon;


    [Header("Spawn Types")]
    public Sprite gemIcon;
    public Sprite coinIcon;
    public Sprite treasureChestIcon;
    public Sprite shieldIcon;
    public Sprite cloakIcon;
    public Sprite timeDilationIcon;
    public Sprite stormboltIcon;
    public Sprite bloodrootIcon;
    public Sprite fireballIcon;
    public Sprite ashbindIcon;
    public Sprite healthIcon;
    public Sprite keyIcon;
    public Sprite playerIcon;


    [Header("Enemy Spawns")]
    public Sprite ruinwalkerIcon;
    public Sprite gravecallerIcon;
    public Sprite bloodseekerIcon;


    [Header("Waypoints")]
    public Sprite ruinwalker1Icon;
    public Sprite ruinwalker2Icon;
    public Sprite ruinwalker3Icon;
    public Sprite ruinwalker4Icon;
    public Sprite gravecaller1Icon;
    public Sprite gravecaller2Icon;
    public Sprite gravecaller3Icon;
    public Sprite gravecaller4Icon;
    public Sprite bloodseeker1Icon;
    public Sprite bloodseeker2Icon;
    public Sprite bloodseeker3Icon;
    public Sprite bloodseeker4Icon;


    [Header("Bridge")]
    public Sprite bridgeIcon;




    public Sprite GetSegmentTypeIcon(int index)
    {
        return index switch
        {
            1 => gapIcon,
            2 => crumblingIcon,
            3 => spikeIcon,
            _ => null
        };
    }


    public Sprite GetSpawnTypeIcon(SpawnType type)
    {
        return type switch
        {
            SpawnType.Gem => gemIcon,
            SpawnType.Coin => coinIcon,
            SpawnType.TreasureChest => treasureChestIcon,
            SpawnType.Shield => shieldIcon,
            SpawnType.Cloak => cloakIcon,
            SpawnType.TimeDilation => timeDilationIcon,
            SpawnType.Stormbolt => stormboltIcon,
            SpawnType.Bloodroot => bloodrootIcon,
            SpawnType.Fireball => fireballIcon,
            SpawnType.Ashbind => ashbindIcon,
            SpawnType.Health => healthIcon,
            SpawnType.Key => keyIcon,
            SpawnType.Player => playerIcon,
            _ => null
        };
    }


    public Sprite GetEnemySpawnTypeIcon(EnemySpawnType type)
    {
        return type switch
        {
            EnemySpawnType.Ruinwalker => ruinwalkerIcon,
            EnemySpawnType.Gravecaller => gravecallerIcon,
            EnemySpawnType.Bloodseeker => bloodseekerIcon,
            _ => null
        };
    }


    public Sprite GetWaypointTypeIcon(WaypointType type)
    {
        return type switch
        {
            WaypointType.Ruinwalker1 => ruinwalker1Icon,
            WaypointType.Ruinwalker2 => ruinwalker2Icon,
            WaypointType.Ruinwalker3 => ruinwalker3Icon,
            WaypointType.Ruinwalker4 => ruinwalker4Icon,
            WaypointType.Gravecaller1 => gravecaller1Icon,
            WaypointType.Gravecaller2 => gravecaller2Icon,
            WaypointType.Gravecaller3 => gravecaller3Icon,
            WaypointType.Gravecaller4 => gravecaller4Icon,
            WaypointType.Bloodseeker1 => bloodseeker1Icon,
            WaypointType.Bloodseeker2 => bloodseeker2Icon,
            WaypointType.Bloodseeker3 => bloodseeker3Icon,
            WaypointType.Bloodseeker4 => bloodseeker4Icon,
            _ => null
        };
    }
}