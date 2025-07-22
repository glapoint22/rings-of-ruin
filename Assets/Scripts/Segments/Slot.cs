using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform waypoint;

    public Transform Waypoint => waypoint;
    public Transform SpawnPoint => spawnPoint;
}