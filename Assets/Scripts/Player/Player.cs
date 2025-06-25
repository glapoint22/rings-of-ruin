using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private PlayerState playerState;

    private void Awake() {
        playerState = new PlayerState
        {
            health = 100
        };
    }

    private void OnEnable() {
        GameEvents.OnCollect += OnCollect;
        GameEvents.OnPickup += OnPickup;
        GameEvents.OnDamage += OnDamage;
    }


    private void OnCollect(IPlayerState state) {
        playerState = state.UpdateState(playerState);
        GameEvents.RaiseCollectionUpdate(playerState);
    }



    private void OnPickup(IPlayerState state, PickupType pickupType) {
        playerState = state.UpdateState(playerState);
        GameEvents.RaisePickupUpdate(pickupType);

        // Check if this is a time-based buff and start coroutine
        if (state is ITimeBasedBuff timeBasedBuff)
        {
            StartCoroutine(HandleTimeBasedBuff(timeBasedBuff));
        }
    }


    private IEnumerator HandleTimeBasedBuff(ITimeBasedBuff buff)
    {
        yield return new WaitForSeconds(buff.Duration);
        
        // Call the buff's expiration method
        playerState = buff.OnBuffExpired(playerState);
    }

    private void OnDamage(Damage damage) {
        playerState = damage.UpdateState(playerState);
        Debug.Log(playerState.health);
    }
}