using UnityEngine;

public class ShieldSpell : MonoBehaviour
{
    [SerializeField] private int shieldHealthValue;
    private PlayerState shieldUpdate;


    private void Awake()
        {
                shieldUpdate = new PlayerState
                {
                        shieldHealth = shieldHealthValue
                };
        }


    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.Shield);
        GameEvents.RaisePlayerStateUpdate(shieldUpdate);
    }
}