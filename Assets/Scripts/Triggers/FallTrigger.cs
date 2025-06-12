using System.Drawing;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DamageInfo damageInfo = new DamageInfo
            {
                DamageType = DamageType.Fall,
            };

            DamageEventManager.DealDamage(damageInfo);
        }
    }
}
