using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement target fields
    private int targetRingIndex;
    private float targetAngle;
    private bool isMoving = false;

    // Current position fields
    private int currentRingIndex = 0;
    private float currentAngle = 90f; // Start at 12:00 position
    [SerializeField] private float moveSpeed = 90f; // degrees per second

    // Pathfinding
    private Pathfinder pathfinder;

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
        Debug.Log("Pathfinder created for level!");
        
       
        
        // Test A* pathfinding
        var path = pathfinder.FindPath(0, 90f, 2, 270f); // From Ring 0, 90° to Ring 2, 270°
        Debug.Log($"A* Path found with {path.Count} steps:");
        foreach (var step in path)
        {
            Debug.Log($"  Ring {step.ringIndex}, Angle {step.angle}°");
        }
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
                float ringRadius = RingConstants.BaseRadius + ringSegment.RingIndex * RingConstants.RingSpacing;
                float angleRad = Mathf.Atan2(hit.point.z, hit.point.x);
                float angleDeg = angleRad * Mathf.Rad2Deg;
                
                // Store the movement target
                targetRingIndex = ringSegment.RingIndex;
                targetAngle = angleDeg;
                isMoving = true;
                
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

    private void Start()
    {
        // Set initial position at 12:00 on ring 0
        UpdatePlayerPosition();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Calculate the shortest path to the target angle
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            
            
            // Move the current angle toward the target
            float step = moveSpeed * Time.deltaTime;
            if (Mathf.Abs(angleDifference) <= step)
            {
                // Close enough, snap to target
                currentAngle = targetAngle;
                isMoving = false;
            }
            else
            {
                // Move in the direction of the shortest path
                currentAngle += Mathf.Sign(angleDifference) * step;
            }
            
            // Update the player's position
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        float ringRadius = RingConstants.BaseRadius + currentRingIndex * RingConstants.RingSpacing;
        float angleRad = currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * ringRadius;
        float z = Mathf.Sin(angleRad) * ringRadius;
        transform.position = new Vector3(x, 0f, z);
    }

    // [SerializeField] private float moveSpeed;
    // private int ringIndex = 0;
    // private float currentAngle = 0f;
    // private float currentRingRadius;
    // private int maxRingIndex = 0;
    // private bool canAccessCenter = false;
    // private int levelGemCount = 0;
    // private Vector3 previousPosition;

    // private void OnEnable()
    // {
    //     GameEvents.OnLevelLoaded += OnLevelLoaded;
    //     GameEvents.OnAddCollectible += OnAddCollectible;
    //     GameEvents.OnPlayerPlacement += OnPlayerPlacement;
    // }

    // private void Start()
    // {
    //     // Calculate and set the player's position
    //     Vector3 position = CalculatePositionAtRadius();
    //     transform.position = position;
    // }

    // private Vector3 CalculatePositionAtRadius()
    // {
    //     // If in center, return center position
    //     if (ringIndex == -1) return Vector3.zero;

    //     float angleRad = (90f - currentAngle) * Mathf.Deg2Rad;
    //     float x = Mathf.Cos(angleRad) * currentRingRadius;
    //     float z = Mathf.Sin(angleRad) * currentRingRadius;
    //     return new Vector3(x, 0f, z);
    // }

    // private void Update()
    // {
    //     UpdateCurrentAngle();
    //     UpdatePosition();
    //     UpdateRotation();
    //     HandleInput();
    // }

    // private void UpdateCurrentAngle()
    // {
    //     // Don't move if in center (level complete)
    //     if (ringIndex == -1) return;

    //     // Convert linear speed to angular speed
    //     float circumference = 2 * Mathf.PI * currentRingRadius;
    //     float degreesPerSecond = moveSpeed / circumference * 360f;

    //     // Update the angle
    //     currentAngle += degreesPerSecond * Time.deltaTime;
    //     currentAngle %= 360f; // Keep angle between 0-360
    // }

    // private void UpdatePosition()
    // {
    //     Vector3 position = CalculatePositionAtRadius();
    //     transform.position = position;
    // }

    // private void UpdateRotation()
    // {
    //     // Calculate movement direction
    //     Vector3 moveDirection = (transform.position - previousPosition).normalized;

    //     // Apply rotation if moving
    //     if (moveDirection != Vector3.zero)
    //     {
    //         transform.rotation = Quaternion.LookRotation(moveDirection);
    //     }

    //     // Update previous position for next frame
    //     previousPosition = transform.position;
    // }

    // private void HandleInput()
    // {
    //     // Don't handle input if in center (level complete)
    //     if (ringIndex == -1) return;

    //     if (Input.GetKeyDown(KeyCode.W)) ChangeRing(-1);
    //     if (Input.GetKeyDown(KeyCode.S)) ChangeRing(1);
    // }


    // private void SetCurrentRingRadius()
    // {
    //     currentRingRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;
    // }

    // private void ChangeRing(int direction)
    // {
    //     int targetRing = ringIndex + direction;

    //     // Handle center access (ringIndex = -1 represents center)
    //     if (targetRing == -1)
    //     {
    //         if (canAccessCenter)
    //         {
    //             ringIndex = -1; // Center - level complete
    //         }
    //         return;
    //     }

    //     // Handle ring bounds
    //     if (targetRing >= 0 && targetRing <= maxRingIndex)
    //     {
    //         ringIndex = targetRing;
    //         SetCurrentRingRadius();
    //     }
    // }

    // private void OnLevelLoaded(LevelData levelData)
    // {
    //     // Set the maximum ring index based on level data
    //     maxRingIndex = levelData.rings.Count - 1;

    //     // Reset player to starting position
    //     ringIndex = 0;
    //     currentAngle = 0f;
    //     SetCurrentRingRadius();
    //     levelGemCount = levelData.GemCount;

    //     // Reset center access (will be enabled when gems collected)
    //     canAccessCenter = false;
    // }


    // private void OnAddCollectible(PlayerState state)
    // {
    //     if (state.gems == levelGemCount)
    //     {
    //         canAccessCenter = true;
    //     }
    // }

    // private void OnPlayerPlacement(int ringIndex, int segmentIndex)
    // {
    //     this.ringIndex = ringIndex;
    //     this.currentAngle = segmentIndex * (360f / RingConstants.SegmentCount);

    //     SetCurrentRingRadius();
    //     Vector3 position = CalculatePositionAtRadius();
    //     transform.position = position;
    // }
}