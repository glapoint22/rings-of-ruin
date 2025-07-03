using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Targeting Settings")]
    [SerializeField] private float lineOfSightAngle = 90f; // 45° left and right = 90° total

    private List<Targetable> registeredTargets = new List<Targetable>();
    private Targetable currentTarget;

    private void OnEnable()
    {
        GameEvents.OnTargetRegistered += RegisterTarget;
        GameEvents.OnTargetUnregistered += UnregisterTarget;

        // Subscribe to input events
        GameEvents.OnTabPressed += () => CycleToNextTarget(false);  // Forward cycling
        GameEvents.OnShiftTabPressed += () => CycleToNextTarget(true);  // Reverse cycling
        GameEvents.OnMouseTargetPressed += SelectTargetAtPosition;
        GameEvents.OnEscapePressed += ClearCurrentTarget;
    }

    private void RegisterTarget(Targetable target)
    {
        if (!registeredTargets.Contains(target))
        {
            registeredTargets.Add(target);
        }
    }

    private void UnregisterTarget(Targetable target)
    {
        if (registeredTargets.Contains(target))
        {
            registeredTargets.Remove(target);

            // If the unregistered target was our current target, clear the selection
            if (currentTarget == target)
            {
                ClearCurrentTarget();
            }
        }
    }

    private List<Targetable> GetVisibleTargetsSortedByDistance(bool reverse = false)
    {
        List<Targetable> visibleTargets = new List<Targetable>();

        foreach (Targetable target in registeredTargets)
        {
            if (IsInLineOfSight(target))
            {
                visibleTargets.Add(target);
            }
        }

        // Sort by distance (closest first)
        visibleTargets.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(playerTransform.position, a.TargetPosition);
            float distanceB = Vector3.Distance(playerTransform.position, b.TargetPosition);
            return distanceA.CompareTo(distanceB);
        });

        // Reverse if requested (farthest first)
        if (reverse)
        {
            visibleTargets.Reverse();
        }

        return visibleTargets;
    }

    private bool IsInLineOfSight(Targetable target)
    {
        // Get direction from player to target
        Vector3 directionToTarget = (target.TargetPosition - playerTransform.position).normalized;

        // Get player's forward direction
        Vector3 playerForward = playerTransform.forward;

        // Calculate angle between player forward and direction to target
        float angle = Vector3.Angle(playerForward, directionToTarget);

        // Check if target is within the line of sight angle (half on each side)
        return angle <= lineOfSightAngle * 0.5f;
    }

    private void CycleToNextTarget(bool reverse = false)
    {
        List<Targetable> visibleTargets = GetVisibleTargetsSortedByDistance(reverse);

        // If no visible targets, keep current selection
        if (visibleTargets.Count == 0)
        {
            return;
        }

        // If no current target, select the first one
        if (currentTarget == null)
        {
            AssignCurrentTarget(visibleTargets[0]);
            return;
        }

        // Find current target in the list
        int currentIndex = visibleTargets.IndexOf(currentTarget);

        // If current target is not in visible list, select the first one
        if (currentIndex == -1)
        {
            AssignCurrentTarget(visibleTargets[0]);
            return;
        }

        // Move to next target (with wrap-around)
        int nextIndex = (currentIndex + 1) % visibleTargets.Count;
        AssignCurrentTarget(visibleTargets[nextIndex]);
    }

    private void SelectTargetAtPosition(Vector3 worldPosition)
    {
        // Cast a ray from camera through the world position
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(worldPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has a Targetable component
            Targetable targetable = hit.collider.GetComponent<Targetable>();
            if (targetable != null && registeredTargets.Contains(targetable))
            {
                AssignCurrentTarget(targetable);
            }
        }
    }


    private void AssignCurrentTarget(Targetable target)
    {
        if (currentTarget != null) currentTarget.HideUI();
        currentTarget = target;
        currentTarget.ShowUI(); // Show UI for new target
    }

    private void ClearCurrentTarget()
    {
        // Hide UI for current target
        if (currentTarget != null)
        {
            currentTarget.HideUI();
        }
        currentTarget = null;
    }
}