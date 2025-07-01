using UnityEngine;

public class AshbindCast : MonoBehaviour
{
    public void OnCast()
    {
        GameEvents.RaiseAddBuff(BuffType.Ashbind);
    }
}