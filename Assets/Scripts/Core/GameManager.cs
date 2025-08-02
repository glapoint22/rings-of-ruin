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

    private LevelBuilder levelBuilder;


    private void Awake()
    {
        levelBuilder = new LevelBuilder(levelPool, levelRoot, navMeshSurface);
        levelPool.Initialize(levelRoot);
    }


    


    public void LoadLevel()
    {
        levelBuilder.BuildLevel(levels[0]);
        StartCoroutine(SpawnEntities());
        
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
}