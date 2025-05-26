using UnityEngine;

[CreateAssetMenu(menuName = "Rings of Ruin/Prefab Library")]
public class LevelPrefabLibrary : ScriptableObject
{
    [Header("Segments")]
    public GameObject[] normalSegmentPrefabs = new GameObject[4];
    public GameObject[] gapSegmentPrefabs = new GameObject[4];

    [Header("Collectibles")]
    public GameObject gemPrefab;
    public GameObject coinPrefab;

    [Header("Pickups")]
    public GameObject shieldPrefab;
    public GameObject cloakPrefab;
    public GameObject timeDilationPrefab;
    public GameObject healthPrefab;
    public GameObject decoyPrefab;
    public GameObject pathmakerPrefab;
    public GameObject spellPrefab;
    public GameObject keyPrefab;

    [Header("Hazards")]
    public GameObject crusherPrefab;
    public GameObject catapultPrefab;
    public GameObject crumblingPlatformPrefab;

    [Header("Enemies")]
    public GameObject ruinwalkerPrefab;
    public GameObject gravecallerPrefab;
    public GameObject bloodseekerPrefab;

    [Header("Checkpoint")]
    public GameObject checkpointPrefab;


    [Header("Portal")]
    public GameObject portalPrefab;


    public GameObject GetCollectiblePrefab(CollectibleType collectibleType)
    {
        GameObject prefab = null;

        if (collectibleType == CollectibleType.Gem)
        {
            prefab = gemPrefab;
        }
        else if (collectibleType == CollectibleType.Coin)
        {
            prefab = coinPrefab;
        }

        return prefab;
    }



    public GameObject GetPickupPrefab(PickupType pickupType)
    {
        GameObject prefab = null;
        switch (pickupType)
        {
            case PickupType.Shield:
                prefab = shieldPrefab;
                break;
            case PickupType.Cloak:
                prefab = cloakPrefab;
                break;
            case PickupType.TimeDilation:
                prefab = timeDilationPrefab;
                break;
            case PickupType.Health:
                prefab = healthPrefab;
                break;
            case PickupType.Decoy:
                prefab = decoyPrefab;
                break;
            case PickupType.Pathmaker:
                prefab = pathmakerPrefab;
                break;
            case PickupType.Key:
                prefab = keyPrefab;
                break;

            case PickupType.Spell:
                prefab = spellPrefab;
                break;
        }

        return prefab;
    }



    public GameObject GetHazardPrefab(HazardType hazardType)
    {
        GameObject prefab = null;

        switch (hazardType)
        {
            case HazardType.Crusher:
                prefab = crusherPrefab;
                break;
            case HazardType.Catapult:
                prefab = catapultPrefab;
                break;
        }

        return prefab;
    }



    public GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        GameObject prefab = null;
        switch (enemyType)
        {
            case EnemyType.Ruinwalker:
                prefab = ruinwalkerPrefab;
                break;
            case EnemyType.Gravecaller:
                prefab = gravecallerPrefab;
                break;
            case EnemyType.Bloodseeker:
                prefab = bloodseekerPrefab;
                break;
        }
        return prefab;
    }
}