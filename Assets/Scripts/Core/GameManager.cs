using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LevelLoader levelLoader;

    private int currentLevelIndex = 0;


    private void OnEnable() {
        GameEvents.OnLevelCompleted += OnLevelCompleted;
    }


    public void LoadLevel() {
        levelLoader.LoadLevel(currentLevelIndex);
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

}