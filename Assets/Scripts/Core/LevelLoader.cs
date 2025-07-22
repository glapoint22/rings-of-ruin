using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelBuilderOld levelBuilder;
    [SerializeField] private List<LevelDataOld> levels;


    public LevelDataOld LoadLevel(int levelIndex)
    {
        LevelDataOld levelData = levels[levelIndex];
        if (levelBuilder == null || levelData == null) return null;

        levelBuilder.BuildLevel(levelData);
        GameEvents.RaiseLevelLoaded(levelData);
        return levelData;
    }
}