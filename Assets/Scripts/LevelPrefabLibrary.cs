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
    public GameObject portalPrefab;
    public GameObject spellSpawnVisual; // Optional visual if needed

    [Header("Hazards")]
    public GameObject crusherPrefab;
    public GameObject catapultPrefab;
    public GameObject crumblingPlatformPrefab;

    [Header("Enemies")]
    public GameObject ruinwalkerPrefab;
    public GameObject gravecallerPrefab;
    public GameObject bloodseekerPrefab;

    [Header("Checkpoint")]
    public GameObject checkpointFlagPrefab;
}