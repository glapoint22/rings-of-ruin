using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;
using System.Collections;

public class GameManager : MonoBehaviour
{

    [SerializeField] private List<LevelData> levels;
    [SerializeField] private LevelPool levelPool;
    [SerializeField] private Transform levelRoot;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private int currentLevelIndex = 0;
    private LevelBuilder levelBuilder;


    private void Awake()
    {
        levelBuilder = new LevelBuilder(levelPool, levelRoot, navMeshSurface);
        levelPool.Initialize(levelRoot);
    }


    private void OnEnable()
    {
        GameEvents.OnLevelCompleted += OnLevelCompleted;
    }


    public void LoadLevel()
    {
        // PauseGame();
        levelBuilder.BuildLevel(levels[currentLevelIndex]);
        GameEvents.RaiseLevelLoaded(levels[currentLevelIndex]);
        levelBuilder.SpawnPlayer();
    }

    


    public void PlayGame()
    {
        Time.timeScale = 1f; // Resume game time
    }


    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game time
    }

    private void OnLevelCompleted()
    {
        currentLevelIndex++;
    }

}