using UnityEngine;
using System.Collections;

public class CloakSpell : MonoBehaviour
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