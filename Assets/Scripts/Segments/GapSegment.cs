using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Collider))]
public class GapSegment : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                //player?.TriggerFall();
            }
        }
    }
}