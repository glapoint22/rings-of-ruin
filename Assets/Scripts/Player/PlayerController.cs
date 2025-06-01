using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the player's movement around concentric rings in a circular pattern.
/// The player automatically moves clockwise and can transition between rings.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Ring Position")]
    [SerializeField] private int ringIndex = 0; // 0 = innermost ring

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f; // Full ring = 24 "units" per second

    // Current angle in degrees (0 to 360)
    private float currentAngle = 0f;
    private Vector3 previousPosition;
    private bool isTransitioning = false;
    private float currentRingRadius;
    
    // Cached vectors to reduce garbage collection
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;

    /// <summary>
    /// Initializes the player's position on the starting ring.
    /// </summary>
    private void Start()
    {
        currentRingRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;
        currentPosition = CalculatePositionAtRadius(currentRingRadius);
        previousPosition = currentPosition;
        transform.position = currentPosition;
    }

    /// <summary>
    /// Updates player movement, position, rotation, and handles input each frame.
    /// </summary>
    private void Update()
    {
        UpdateAutomaticMovement();
        UpdatePosition();
        UpdateRotation();
        HandleInput();
    }

    /// <summary>
    /// Handles the automatic clockwise movement around the current ring.
    /// Calculates the angular velocity based on the current ring's circumference.
    /// </summary>
    private void UpdateAutomaticMovement()
    {
        float circumference = 2 * Mathf.PI * currentRingRadius;
        float degreesPerSecond = (moveSpeed / circumference) * 360f;
        currentAngle += degreesPerSecond * Time.deltaTime;
        currentAngle %= 360f; // Wrap around to keep within 0-360 degrees
    }

    /// <summary>
    /// Updates the player's position based on the current angle and ring radius.
    /// </summary>
    private void UpdatePosition()
    {
        currentPosition = CalculatePositionAtRadius(currentRingRadius);
        transform.position = currentPosition;
        previousPosition = currentPosition;
    }

    /// <summary>
    /// Updates the player's rotation to face the direction of movement.
    /// </summary>
    private void UpdateRotation()
    {
        // Calculate movement direction based on position change
        moveDirection = (currentPosition - previousPosition).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    /// <summary>
    /// Handles player input for ring transitions.
    /// W/S keys move inward/outward respectively.
    /// </summary>
    private void HandleInput()
    {
        if (!isTransitioning)
        {
            if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(SmoothRingTransition(-1));
            if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(SmoothRingTransition(1));
        }
    }

    /// <summary>
    /// Calculates the world position at a given radius using the current angle.
    /// Reuses the currentPosition vector to avoid garbage collection.
    /// </summary>
    private Vector3 CalculatePositionAtRadius(float radius)
    {
        float angleRad = (450f - currentAngle) * Mathf.Deg2Rad;
        currentPosition.x = Mathf.Cos(angleRad) * radius;
        currentPosition.y = 0f;
        currentPosition.z = Mathf.Sin(angleRad) * radius;
        return currentPosition;
    }

    /// <summary>
    /// Smoothly transitions the player between rings.
    /// </summary>
    /// <param name="direction">-1 for inward, 1 for outward</param>
    private IEnumerator SmoothRingTransition(int direction)
    {
        int targetRing = ringIndex + direction;
        if (targetRing < 0 || targetRing > 3) yield break;

        isTransitioning = true;

        // Calculate start and end radii for the transition
        float startRadius = currentRingRadius;
        float endRadius = RingConstants.BaseRadius + targetRing * RingConstants.RingSpacing;
        float duration = 0.5f;
        float elapsed = 0f;

        // Smoothly interpolate between rings
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = EaseInOut(t);
            float currentRadius = Mathf.Lerp(startRadius, endRadius, easedT);
            
            transform.position = CalculatePositionAtRadius(currentRadius);
            yield return null;
        }

        // Finalize the transition
        ringIndex = targetRing;
        currentRingRadius = endRadius;
        transform.position = CalculatePositionAtRadius(currentRingRadius);

        UpdatePosition();
        UpdateRotation();
        isTransitioning = false;
    }

    /// <summary>
    /// Applies smoothstep easing to the transition.
    /// </summary>
    private float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t); // smoothstep
    }


    public void FallIntoGap()
    {
        Debug.Log("[PlayerController] Falling into gap...");

        // Stop movement
        enabled = false;

        // Enable gravity
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}