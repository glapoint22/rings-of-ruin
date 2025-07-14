using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private Coroutine currentMovementCoroutine;
    private List<Vector3> currentPath;
    private Vector3 currentPosition;
    public Vector3 CurrentPosition => currentPosition;

    public void SetStartPosition(Vector3 startPosition)
    {
        currentPosition = startPosition;
    }

    public void MoveAlongPath(List<Vector3> path)
    {
        // Stop any current movement
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        // Store the new path and start movement
        currentPath = path;
        currentMovementCoroutine = StartCoroutine(MoveAlongPathCoroutine());
    }

    private IEnumerator MoveAlongPathCoroutine()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            yield break;
        }

        // Move to each path point
        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 targetPosition = currentPath[i];
            
            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Calculate movement direction
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                
                // Move the object
                currentPosition = transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                
                // Rotate to face movement direction
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                
                yield return null;
            }
        }

        // Clear movement state
        currentMovementCoroutine = null;
        currentPath = null;
        
        // Raise path completion event
        GameEvents.RaisePathCompleted();
    }
}