using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LevelLoader levelLoader;

    private int levelGemCount;
    private int currentLevelIndex = 0;


    // private void Awake()
    // {
    //     PauseGame(); // Start with the game paused

    // }


    // private void Start() {
    //     LevelData levelData = levelLoader.LoadLevel(currentLevelIndex);
    //     SetGemCount(levelData);
    // }


    private void OnEnable() {
        GameEvents.OnCollectionUpdate += OnCollectionUpdate;
        GameEvents.OnLevelCompleted += OnLevelCompleted;
    }


    public void LoadLevel() {
        LevelData levelData = levelLoader.LoadLevel(currentLevelIndex);
        SetGemCount(levelData);
        PauseGame();
    }


    public void PlayGame()
    {
        Time.timeScale = 1f; // Resume game time
    }


    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game time
    }

    private void OnLevelCompleted() {
        currentLevelIndex++;
    }


    private void OnCollectionUpdate(PlayerState state) {
        if(state.gems == levelGemCount) {
            // currentLevelIndex++;
            // LevelData levelData = levelLoader.LoadLevel(currentLevelIndex);
            // SetGemCount(levelData);
        }
    }


    private void SetGemCount(LevelData levelData) {
        levelGemCount = levelData.GemCount;
    }




}