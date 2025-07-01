using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState playerState = new ();

    private void OnEnable()
    {
        GameEvents.OnDamage += OnDamage;
        GameEvents.OnLevelLoaded += OnLevelLoaded;
        GameEvents.OnPlayerStateUpdate += OnPlayerStateUpdate;
    }


    private void OnPlayerStateUpdate(PlayerState state)
    {
        playerState.Update(state);
    }


    private void OnDamage(Damage damage)
    {
        damage.UpdateState(playerState);
    }

    private void OnLevelLoaded(LevelData levelData)
    {
        playerState.gems = 0;
        GameEvents.RaiseAddCollectible(playerState);
    }
}