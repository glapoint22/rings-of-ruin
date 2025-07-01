using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerState playerState = new ();

    private void OnEnable()
    {
        GameEvents.OnCollect += OnCollect;
        GameEvents.OnPlayerStateUpdate += OnPickup;
        GameEvents.OnDamage += OnDamage;
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }


    private void OnCollect(PlayerState stateUpdate)
    {
        playerState.Update(stateUpdate);
    }



    private void OnPickup(PlayerState state)
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
        GameEvents.RaiseCollectionUpdate(playerState);
    }
}