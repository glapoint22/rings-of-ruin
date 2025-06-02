using UnityEngine;

/// <summary>
/// Rotates the entire ring GameObject around the Y-axis based on the assigned rotation direction.
/// Direction is injected at runtime from LevelBuilder via RingConfiguration.
/// </summary>
public class RotatingRing : MonoBehaviour
{
    private RingRotationDirection rotationDirection = RingRotationDirection.None;

    [SerializeField]
    private float rotationSpeed = 10f; // Degrees per second

    private void Update()
    {
        if (rotationDirection == RingRotationDirection.None)
            return;

        float directionMultiplier = (rotationDirection == RingRotationDirection.Clockwise) ? -1f : 1f;
        transform.Rotate(Vector3.up, directionMultiplier * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Injected by LevelBuilder after reading RingConfiguration.
    /// </summary>
    public void SetRotationDirection(RingRotationDirection direction)
    {
        rotationDirection = direction;
    }

    /// Optional helpers if needed elsewhere
    public RingRotationDirection GetDirection() => rotationDirection;
    public float GetDirectionMultiplier() => (rotationDirection == RingRotationDirection.Clockwise) ? 1f : -1f;
    public bool IsRotating() => rotationDirection != RingRotationDirection.None;
}
