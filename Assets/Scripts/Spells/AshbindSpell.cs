using UnityEngine;

public class AshbindSpell : MonoBehaviour
{
    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.Ashbind);
    }
}