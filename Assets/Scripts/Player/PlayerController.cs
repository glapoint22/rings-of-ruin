using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Pathfinder pathfinder;
    private PathMover pathMover;

    private void OnEnable()
    {
        GameEvents.OnRightMousePressed += HandleRightMousePressed;
        GameEvents.OnLevelLoaded += OnLevelLoaded;
    }

    

    private void OnLevelLoaded(LevelData levelData)
    {
        pathfinder = new Pathfinder(levelData);
        pathMover = GetComponent<PathMover>();
        pathMover.SetStartPosition(transform.position);
    }

    private void HandleRightMousePressed(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            RingSegment ringSegment = hit.collider.GetComponentInParent<RingSegment>();
            if (ringSegment != null)
            {
                var path = pathfinder.GetPath(pathMover.CurrentPosition, hit.point);
                // Debug: Draw the path
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.green, 5f);
                }
                pathMover.MoveAlongPath(path);
            }
        }
    }
}
