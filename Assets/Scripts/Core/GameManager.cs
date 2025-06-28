using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LevelLoader levelLoader;

    private int gemCount;
    private int currentLevelIndex = 0;


    private void Awake()
    {
        PauseGame(); // Start with the game paused

    }


    private void Start() {
        levelLoader.LoadLevel(currentLevelIndex);
    }


    private void OnEnable() {
        GameEvents.OnCollectionUpdate += OnCollectionUpdate;
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }


    public void PlayGame()
    {
        Time.timeScale = 1f; // Resume game time
    }


    public void PauseGame()
    {
        Time.timeScale = 0f; // Pause game time
    }


    private void OnCollectionUpdate(PlayerState state) {
        if(state.gems == gemCount) {
            Debug.Log("Level Complete");
            currentLevelIndex++;
            levelLoader.LoadLevel(currentLevelIndex);
        }
    }


    private void OnLevelLoaded(LevelData levelData) {
        gemCount = levelData.GemCount;
    }




}