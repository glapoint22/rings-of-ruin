using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedPanel;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelCompletedPanel.SetActive(true);
            GameEvents.RaiseLevelCompleted();
        }
    }
}