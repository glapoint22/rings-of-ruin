using UnityEngine;
using System.Collections;

public class CloakCast : MonoBehaviour
{
    [SerializeField] private float duration;


    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.Cloak);
        StartCoroutine(HandleTimeBasedBuff());
    }


    private IEnumerator HandleTimeBasedBuff()
    {
        yield return new WaitForSeconds(duration);
        GameEvents.RaiseRemoveBuff(BuffType.Cloak);
    }
}