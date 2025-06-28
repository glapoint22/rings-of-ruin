using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Ring Position")]
    [SerializeField] private int ringIndex = 0; // 0 = innermost ring

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f; // Full ring = 24 "units" per second


    [Header("Rotation Influence Settings")]
    [SerializeField] private float clockwiseBoost = 1.2f;
    [SerializeField] private float counterClockwiseDrag = 0.7f;

    // Current angle in degrees (0 to 360)
    private float currentAngle = 0f;
    private Vector3 previousPosition;
    private bool isTransitioning = false;
    private float currentRingRadius;

    // Cached vectors to reduce garbage collection
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;





    private void Start()
    {
        currentRingRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;
        currentPosition = CalculatePositionAtRadius(currentRingRadius);
        previousPosition = currentPosition;
        transform.position = currentPosition;
    }



    public void Update()
    {
        UpdateAutomaticMovement();
        UpdatePosition();
        UpdateRotation();
        HandleInput();
    }




    private void UpdateAutomaticMovement()
    {
        float rotationMultiplier = GetRingRotationSpeedMultiplier();

        float circumference = 2 * Mathf.PI * currentRingRadius;
        float degreesPerSecond = (moveSpeed * rotationMultiplier / circumference) * 360f;

        currentAngle += degreesPerSecond * Time.deltaTime;
        currentAngle %= 360f;
    }



    private float GetRingRotationSpeedMultiplier()
    {
        // Default multiplier is neutral
        float multiplier = 1f;

        // Try to get the ring root transform
        if (LevelBuilder.RingRoots.TryGetValue(ringIndex, out Transform ringRoot))
        {
            RotatingRing rotatingRing = ringRoot.GetComponent<RotatingRing>();
            if (rotatingRing != null)
            {
                RingRotationDirection direction = rotatingRing.GetDirection();

                if (direction == RingRotationDirection.Clockwise)
                    multiplier = clockwiseBoost; // Boost
                else if (direction == RingRotationDirection.CounterClockwise)
                    multiplier = counterClockwiseDrag; // Drag
            }
        }

        return multiplier;
    }



    private void UpdatePosition()
    {
        currentPosition = CalculatePositionAtRadius(currentRingRadius);
        transform.position = currentPosition;
    }


    private void UpdateRotation()
    {
        // Calculate movement direction based on position change
        moveDirection = (currentPosition - previousPosition).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

        // Update the previous position *after* rotation is calculated
        previousPosition = currentPosition;
    }


    private void HandleInput()
    {
        if (!isTransitioning)
        {
            if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(SmoothRingTransition(-1));
            if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(SmoothRingTransition(1));
        }
    }

    private Vector3 CalculatePositionAtRadius(float radius)
    {
        float angleRad = (450f - currentAngle) * Mathf.Deg2Rad;
        currentPosition.x = Mathf.Cos(angleRad) * radius;
        currentPosition.y = 0f;
        currentPosition.z = Mathf.Sin(angleRad) * radius;
        return currentPosition;
    }



    private IEnumerator SmoothRingTransition(int direction)
    {
        int targetRing = ringIndex + direction;

        // Prevent transition if target ring doesn't exist
        // if (!RingExists(targetRing))
        // {
        //     yield break;
        // }

        isTransitioning = true;

        float startRadius = currentRingRadius;
        float endRadius = RingConstants.BaseRadius + targetRing * RingConstants.RingSpacing;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = EaseInOut(t);
            float currentRadius = Mathf.Lerp(startRadius, endRadius, easedT);

            transform.position = CalculatePositionAtRadius(currentRadius);
            yield return null;
        }

        ringIndex = targetRing;
        currentRingRadius = endRadius;
        transform.position = CalculatePositionAtRadius(currentRingRadius);

        UpdatePosition();
        UpdateRotation();
        isTransitioning = false;
    }



    private bool RingExists(int index)
    {
        return LevelBuilder.RingRoots.ContainsKey(index);
    }




    
    private float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t); // smoothstep
    }


    public void TriggerFall()
    {

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




    public void SetPositionOnRing(int ring, int segment)
    {
        ringIndex = ring;
        currentRingRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;

        float anglePerSegment = 360f / RingConstants.SegmentCount;
        currentAngle = segment * anglePerSegment;

        UpdatePosition();
    }
}