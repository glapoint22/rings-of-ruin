public enum GameState
{
    Idle,       // Default state when game first loads
    Playing,    // Player is active and systems are running
    Paused,     // Game is paused (UI shown, movement frozen)
    GameOver    // (Optional) Can be expanded for end-of-level
}