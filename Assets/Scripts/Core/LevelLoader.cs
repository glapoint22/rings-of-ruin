using System;
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

    public event EventHandler<LevelData> OnLevelLoaded;
    public static LevelLoader instance;


    private LevelData currentLevel;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
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

        //runeflareManager.InitializeFromLevel(levelData);

        OnLevelLoaded?.Invoke(this, levelData);

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