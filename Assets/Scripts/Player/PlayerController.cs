using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isMoving = false;
    //[SerializeField] private float moveSpeed = 90f; // degrees per second

    private Pathfinder pathfinder;
    private int startRingIndex = 0;
    private int startSegmentIndex = 0;

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

                // Debug: Draw the path
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i].position, path[i + 1].position, Color.green, 5f);
                }
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



    private void Update()
    {
        if (isMoving)
        {

        }
    }
}