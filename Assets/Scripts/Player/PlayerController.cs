using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Pathfinder pathfinder;
    private int startRingIndex = 0;
    private int startSegmentIndex = 12;

    // Movement state
    private Coroutine currentMovementCoroutine;
    private List<Path> currentPath;

    private void OnEnable()
    {
        // Subscribe to the right mouse pressed event
        GameEvents.OnRightMousePressed += HandleRightMousePressed;
        // Subscribe to level loaded event
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelData levelData)
    {
        // Create the pathfinder when a level is loaded
        pathfinder = new Pathfinder(levelData);
    }

    private void HandleRightMousePressed(Vector3 screenPosition)
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Try to get the RingSegment component from the hit object or its parent
            RingSegment ringSegment = hit.collider.GetComponentInParent<RingSegment>();
            if (ringSegment != null)
            {
                
                var path = pathfinder.GetPath(startRingIndex, startSegmentIndex, ringSegment.RingIndex, ringSegment.SegmentIndex, hit.point);

                // Start movement along the path
                StartMovement(path);
            }
            else
            {
                Debug.Log("Hit something, but not a ring segment.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    private void StartMovement(List<Path> path)
    {
        // Stop any current movement
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        // Store the new path and start movement
        currentPath = path;
        currentMovementCoroutine = StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            yield break;
        }

        // Move to each path point
        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 targetPosition = currentPath[i].position;
            var lastPathPoint = currentPath[i];
            startRingIndex = lastPathPoint.ringIndex;
            startSegmentIndex = lastPathPoint.segmentIndex;
            
            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Calculate movement direction
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                
                // Move the player
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                
                // Rotate the player to face movement direction
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
                }
                
                yield return null;
            }
        }

        // Clear movement state
        currentMovementCoroutine = null;
        currentPath = null;
    }
}