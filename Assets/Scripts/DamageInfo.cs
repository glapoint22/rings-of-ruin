using UnityEngine;

public struct DamageInfo
{
    public int Amount;
    public GameObject Source;
    public DamageType DamageType;
    public bool CanBeBlockedByShield;
}