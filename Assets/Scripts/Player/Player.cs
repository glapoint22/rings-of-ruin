using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState playerState = new();
    [SerializeField] private Animator animator;

    

    private void OnEnable()
    {
        GameEvents.OnDamage += OnDamage;
        GameEvents.OnPlayerStateUpdate += OnPlayerStateUpdate;
    }


    private void OnPlayerStateUpdate(PlayerState state)
    {
        playerState.Update(state);
    }


    private void OnDamage(Damage damage)
    {
        damage.UpdateState(playerState);
        if (playerState.shieldHealth <= 0) animator.SetTrigger("Hit");
        if (playerState.health <= 0) 
        {
            animator.SetTrigger("Death");
            playerState.isDead = true;
        }
    }
}