using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private LevelBuilder levelBuilder;
    [SerializeField] private List<LevelData> levels;



    // private void Start()
    // {
    //     if (levels != null && levels.Count > 0) LoadLevel(0);

    // }



    public void LoadLevel(int levelIndex)
    {
        LevelData levelData = levels[levelIndex];
        if (levelBuilder == null || levelData == null) return;

        levelBuilder.BuildLevel(levelData);
        GameEvents.RaiseLevelLoaded(levelData);
    }
}