using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [Header("Runtime Level Builder")]
    [SerializeField] private LevelBuilder levelBuilder;

    [Header("Initial Level (optional)")]
    [SerializeField] private LevelData initialLevel;

    [Header("Build Root")]
    [SerializeField] private Transform levelRoot;

    [SerializeField] private RuneflareManager runeflareManager;


    private LevelData currentLevel;

    private void Awake()
    {
        if (initialLevel != null)
        {
            LoadLevel(initialLevel);
        }
    }

    public void LoadLevel(LevelData levelData)
    {
        ClearLevel();
        currentLevel = levelData;

        if (levelBuilder == null || levelRoot == null || levelData == null)
        {
            Debug.LogError("LevelLoader: Missing required references.");
            return;
        }

        levelBuilder.BuildLevel(levelData, levelRoot);
        //Debug.Log($"[LevelLoader] Loaded Level {levelData.levelID}");

        runeflareManager.InitializeFromLevel(levelData);

    }

    private void ClearLevel()
    {
        if (levelRoot == null) return;

        foreach (Transform child in levelRoot)
        {
            Destroy(child.gameObject);
        }
    }

    public LevelData GetCurrentLevel()
    {
        return currentLevel;
    }
}