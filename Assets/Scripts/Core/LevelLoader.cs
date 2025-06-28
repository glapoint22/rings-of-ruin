using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelBuilder levelBuilder;
    [SerializeField] private List<LevelData> levels;


    public LevelData LoadLevel(int levelIndex)
    {
        LevelData levelData = levels[levelIndex];
        if (levelBuilder == null || levelData == null) return null;

        levelBuilder.BuildLevel(levelData);
        GameEvents.RaiseLevelLoaded(levelData);
        return levelData;
    }
}