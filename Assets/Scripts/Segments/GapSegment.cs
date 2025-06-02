using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GapSegment : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    public void OnPlayerStep(PlayerState player)
    {
        Debug.Log("[GapSegment] Player fell into a gap!");

        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.FallIntoGap();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState playerState = other.GetComponent<PlayerState>();
            if (playerState != null)
            {
                OnPlayerStep(playerState);
            }
        }
    }
}