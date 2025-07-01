using UnityEngine;

public class ShieldCast : MonoBehaviour
{
    [SerializeField] private int shieldHealthValue;
    private PlayerState shieldCasted;


    private void Awake()
    {
        shieldCasted = new PlayerState
        {
            shieldHealth = shieldHealthValue
        };
    }


    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.Shield);
        GameEvents.RaisePlayerStateUpdate(shieldCasted);
    }
}