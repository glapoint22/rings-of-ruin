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



   

    public void Update()
    {
        if (rotationDirection == RingRotationDirection.None)
            return;

        float modifier = 1.0f;//TimeDilationSystem.Instance.GetRotationMultiplier(); // e.g., 1.0f normally, 0.5f if slowed
        float directionMultiplier = (rotationDirection == RingRotationDirection.Clockwise) ? 1f : -1f;
        transform.Rotate(Vector3.up, directionMultiplier * rotationSpeed * modifier * Time.deltaTime);
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