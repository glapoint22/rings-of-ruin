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
        GameEvents.OnInteracted += OnInteracted;
    }


    public void LoadLevel()
    {
        levelBuilder.BuildLevel(levels[currentLevelIndex]);
        StartCoroutine(SpawnEntities());
        GameEvents.RaiseLevelLoaded(levels[currentLevelIndex]);
        
        PauseGame();
    }

    private IEnumerator SpawnEntities()
    {
        yield return new WaitForEndOfFrame();
        levelBuilder.SpawnPlayer();
        levelBuilder.SpawnEnemies();
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

    private void OnInteracted(GameObject interactable)
    {
        levelPool.Return(interactable);
    }

}