using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private int ringIndex = 0;
    private float currentAngle = 0f;
    private float currentRingRadius;
    private int maxRingIndex = 0;
    private bool canAccessCenter = false;
    private int levelGemCount = 0;
    private Vector3 previousPosition;

    private void OnEnable()
    {
        GameEvents.OnLevelLoaded += OnLevelLoaded;
        GameEvents.OnAddCollectible += OnAddCollectible;
        GameEvents.OnPlayerPlacement += OnPlayerPlacement;
    }

    private void Start()
    {
        // Calculate and set the player's position
        Vector3 position = CalculatePositionAtRadius();
        transform.position = position;
    }

    private Vector3 CalculatePositionAtRadius()
    {
        // If in center, return center position
        if (ringIndex == -1) return Vector3.zero;

        float angleRad = (90f - currentAngle) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * currentRingRadius;
        float z = Mathf.Sin(angleRad) * currentRingRadius;
        return new Vector3(x, 0f, z);
    }

    private void Update()
    {
        UpdateCurrentAngle();
        UpdatePosition();
        UpdateRotation();
        HandleInput();
    }

    private void UpdateCurrentAngle()
    {
        // Don't move if in center (level complete)
        if (ringIndex == -1) return;

        // Convert linear speed to angular speed
        float circumference = 2 * Mathf.PI * currentRingRadius;
        float degreesPerSecond = moveSpeed / circumference * 360f;

        // Update the angle
        currentAngle += degreesPerSecond * Time.deltaTime;
        currentAngle %= 360f; // Keep angle between 0-360
    }

    private void UpdatePosition()
    {
        Vector3 position = CalculatePositionAtRadius();
        transform.position = position;
    }

    private void UpdateRotation()
    {
        // Calculate movement direction
        Vector3 moveDirection = (transform.position - previousPosition).normalized;
        
        // Apply rotation if moving
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        
        // Update previous position for next frame
        previousPosition = transform.position;
    }

    private void HandleInput()
    {
        // Don't handle input if in center (level complete)
        if (ringIndex == -1) return;

        if (Input.GetKeyDown(KeyCode.W)) ChangeRing(-1);
        if (Input.GetKeyDown(KeyCode.S)) ChangeRing(1);
        
        // Targeting input - more efficient with else if
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                GameEvents.RaiseShiftTabPressed(); // Reverse cycling
            }
            else
            {
                GameEvents.RaiseTabPressed(); // Forward cycling
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEvents.RaiseEscapePressed();
        }
        else if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            GameEvents.RaiseMouseTargetPressed(Input.mousePosition);
        }
    }


    private void SetCurrentRingRadius()
    {
        currentRingRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;
    }

    private void ChangeRing(int direction)
    {
        int targetRing = ringIndex + direction;

        // Handle center access (ringIndex = -1 represents center)
        if (targetRing == -1)
        {
            if (canAccessCenter)
            {
                ringIndex = -1; // Center - level complete
            }
            return;
        }

        // Handle ring bounds
        if (targetRing >= 0 && targetRing <= maxRingIndex)
        {
            ringIndex = targetRing;
            SetCurrentRingRadius();
        }
    }

    private void OnLevelLoaded(LevelData levelData)
    {
        // Set the maximum ring index based on level data
        maxRingIndex = levelData.rings.Count - 1;

        // Reset player to starting position
        ringIndex = 0;
        currentAngle = 0f;
        SetCurrentRingRadius();
        levelGemCount = levelData.GemCount;

        // Reset center access (will be enabled when gems collected)
        canAccessCenter = false;
    }


    private void OnAddCollectible(PlayerState state)
    {
        if (state.gems == levelGemCount)
        {
            canAccessCenter = true;
        }
    }

    private void OnPlayerPlacement(int ringIndex, int segmentIndex)
    {
        this.ringIndex = ringIndex;
        this.currentAngle = segmentIndex * (360f / RingConstants.SegmentCount);
        
        SetCurrentRingRadius();
        Vector3 position = CalculatePositionAtRadius();
        transform.position = position;
    }
}