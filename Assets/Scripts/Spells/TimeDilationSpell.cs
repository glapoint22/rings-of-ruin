using UnityEngine;
using System.Collections;

public class TimeDilationSpell : MonoBehaviour
{
    [SerializeField] private float duration;


    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.TimeDilation);
        StartCoroutine(HandleTimeBasedBuff());
    }


    private IEnumerator HandleTimeBasedBuff()
    {
        yield return new WaitForSeconds(duration);
        GameEvents.RaiseRemoveBuff(BuffType.TimeDilation);
    }
}