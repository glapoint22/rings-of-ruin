using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        PauseGame(); // Start with the game paused
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