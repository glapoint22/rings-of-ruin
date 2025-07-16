using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder
{
    private readonly LevelData levelData;
    private readonly Dictionary<int, List<int>> validSegmentsPerRing;


    public Pathfinder(LevelData levelData)
    {
        this.levelData = levelData;
        this.validSegmentsPerRing = new Dictionary<int, List<int>>();

        // Initialize the lookup structure
        BuildValidSegmentsLookup();
    }



    private void BuildValidSegmentsLookup()
    {
        foreach (var ring in levelData.rings)
        {
            List<int> validSegments = new();

            for (int i = 0; i < ring.segments.Count; i++)
            {
                var segment = ring.segments[i];

                // For now, only Normal segments are valid (ignore crumbling for now)
                if (segment.segmentType == SegmentType.Normal)
                {
                    validSegments.Add(i);
                }
            }
            validSegmentsPerRing[ring.ringIndex] = validSegments;
        }
    }



    public List<Vector3> GetPath(Vector3 startPosition, Vector3 targetPosition)
    {
        int startRingIndex = GetRingIndexFromPosition(startPosition);
        int startSegmentIndex = GetSegmentIndexFromPosition(startPosition);
        int targetRingIndex = GetRingIndexFromPosition(targetPosition);
        int targetSegmentIndex = GetSegmentIndexFromPosition(targetPosition);
        
        float startAngle = GetSegmentAngle(startSegmentIndex);
        float targetAngle = GetSegmentAngle(targetSegmentIndex);

        List<PathNode> openSet = new();
        List<PathNode> closedSet = new();

        // Create start node
        PathNode startNode = new PathNode(startRingIndex, startAngle);
        startNode.gCost = 0;
        startNode.hCost = CalculateHeuristic(startNode, targetRingIndex, targetAngle);

        openSet.Add(startNode);

        // A* main loop
        while (openSet.Count > 0)
        {
            // Find node with lowest fCost
            PathNode parentPathNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < parentPathNode.fCost)
                {
                    parentPathNode = openSet[i];
                }
            }

            // Move parent path node from open to closed set
            openSet.Remove(parentPathNode);
            closedSet.Add(parentPathNode);

            // Check if we reached the target
            if (parentPathNode.ringIndex == targetRingIndex && Mathf.Abs(Mathf.DeltaAngle(parentPathNode.angle, targetAngle)) < 1f)
            {
                return BuildPath(parentPathNode, targetPosition);
            }

            // Explore all adjacent path nodes
            List<AdjacentPathNode> adjacentPathNodes = GetAdjacentPathNodes(parentPathNode.ringIndex, parentPathNode.angle);
            foreach (var adjacentPathNode in adjacentPathNodes)
            {
                // Skip if this adjacent path node is already in closed set
                if (closedSet.Any(node => node.ringIndex == adjacentPathNode.ringIndex && Mathf.Abs(Mathf.DeltaAngle(node.angle, adjacentPathNode.angle)) < 1f))
                    continue;

                // Calculate cost to this adjacent path node
                float moveCost = CalculateMoveCost(parentPathNode, adjacentPathNode);
                float adjacentPathNodeGCost = parentPathNode.gCost + moveCost;

                // Check if the current adjacent PathNode already exists in openSet
                PathNode currentAdjacentPathNode = openSet.FirstOrDefault(node => node.ringIndex == adjacentPathNode.ringIndex && Mathf.Abs(Mathf.DeltaAngle(node.angle, adjacentPathNode.angle)) < 1f);

                // If the current adjacent PathNode does (NOT) exist in openSet, add it
                if (currentAdjacentPathNode == null)
                {
                    // New node - add to open set for potential future exploration
                    PathNode childPathNode = new(adjacentPathNode.ringIndex, adjacentPathNode.angle)
                    {
                        parent = parentPathNode,
                        gCost = adjacentPathNodeGCost
                    };
                    childPathNode.hCost = CalculateHeuristic(childPathNode, targetRingIndex, targetAngle);
                    openSet.Add(childPathNode);
                }

                // This node was already discovered, but we found a cheaper path to reach it
                else if (adjacentPathNodeGCost < currentAdjacentPathNode.gCost)
                {
                    // Update the node to use the cheaper path we just found
                    currentAdjacentPathNode.parent = parentPathNode;
                    currentAdjacentPathNode.gCost = adjacentPathNodeGCost;
                }
            }
        }
        return new List<Vector3>();
    }



    private float GetSegmentAngle(int segmentIndex)
    {
        float angle = 90f - (segmentIndex * 15f);

        // Normalize to 0-360 range
        while (angle < 0) angle += 360f;
        while (angle >= 360f) angle -= 360f;

        return angle;
    }



    private float CalculateHeuristic(PathNode node, int targetRing, float targetAngle)
    {
        return CalculateCost(node.ringIndex, node.angle, targetRing, targetAngle);
    }



    private float CalculateMoveCost(PathNode parentPathNode, AdjacentPathNode adjacentPathNode)
    {
        return CalculateCost(parentPathNode.ringIndex, parentPathNode.angle, adjacentPathNode.ringIndex, adjacentPathNode.angle);
    }



    private float CalculateCost(int fromRing, float fromAngle, int toRing, float toAngle)
    {
        Vector3 fromPosition = GetPosition(fromRing, fromAngle);
        Vector3 toPosition = GetPosition(toRing, toAngle);

        // Cost for distance
        return Vector3.Distance(fromPosition, toPosition);
    }



    private List<AdjacentPathNode> GetAdjacentPathNodes(int currentRing, float currentAngle)
    {
        List<AdjacentPathNode> adjacentPositions = new List<AdjacentPathNode>();

        // Check adjacent angles on the same ring
        float[] adjacentAngles = { currentAngle + 15f, currentAngle - 15f };

        foreach (float angle in adjacentAngles)
        {
            if (IsValidPosition(currentRing, angle))
            {
                adjacentPositions.Add(new AdjacentPathNode(currentRing, angle));
            }
        }

        // Check adjacent rings at the same angle (with bridge validation)
        int[] adjacentRings = { currentRing + 1, currentRing - 1 };

        foreach (int ringIndex in adjacentRings)
        {
            // Only check if the ring index is within valid bounds (0 to max ring)
            if (ringIndex >= 0 && ringIndex < levelData.rings.Count && IsValidPosition(ringIndex, currentAngle))
            {
                // Check for bridge existence based on direction
                bool hasValidBridge = false;
                
                if (ringIndex == currentRing - 1)
                {
                    // Moving inward - check if current segment has bridge
                    int currentSegmentIndex = GetSegmentIndex(currentAngle);
                    var currentSegment = levelData.rings[currentRing].segments[currentSegmentIndex];
                    hasValidBridge = currentSegment.hasBridge;
                }
                else if (ringIndex == currentRing + 1)
                {
                    // Moving outward - check if outer ring's segment has bridge
                    int outerSegmentIndex = GetSegmentIndex(currentAngle);
                    var outerSegment = levelData.rings[ringIndex].segments[outerSegmentIndex];
                    hasValidBridge = outerSegment.hasBridge;
                }

                if (hasValidBridge)
                {
                    adjacentPositions.Add(new AdjacentPathNode(ringIndex, currentAngle));
                }
            }
        }
        return adjacentPositions;
    }



    private bool IsValidPosition(int ringIndex, float angle)
    {
        // Convert angle to segment index (matching the actual segment positioning)
        int segmentIndex = GetSegmentIndex(angle);

        // Check if this ring and segment combination is valid
        if (validSegmentsPerRing.ContainsKey(ringIndex))
        {
            return validSegmentsPerRing[ringIndex].Contains(segmentIndex);
        }
        return false; // Ring doesn't exist
    }



    private int GetSegmentIndex(float angle)
    {
        // Normalize the angle first to match GetSegmentAngle's normalization
        while (angle < 0) angle += 360f;
        while (angle >= 360f) angle -= 360f;

        // Convert angle to segment index (inverse of GetSegmentAngle)
        // GetSegmentAngle: angle = 90f - (segmentIndex * 15f)
        // So: segmentIndex = (90f - angle) / 15f
        int segmentIndex = Mathf.RoundToInt((90f - angle) / 15f);

        // Normalize to 0-23 range
        segmentIndex = segmentIndex % 24;
        if (segmentIndex < 0) segmentIndex += 24;

        return segmentIndex;
    }







    private List<Vector3> BuildPath(PathNode endNode, Vector3 targetPosition)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode currentNode = endNode;

        while (currentNode != null)
        {
            Vector3 position;

            if (path.Count == 0)
            {
                position = targetPosition;
            }
            else
            {
                position = GetPosition(currentNode.ringIndex, currentNode.angle);
            }

            path.Insert(0, position);
            currentNode = currentNode.parent;
        }
        if (path.Count > 1)
            path.RemoveAt(0);

        return path;
    }


    private Vector3 GetPosition(int ringIndex, float angle)
    {
        float ringRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingRadiusOffset;
        float angleRad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * ringRadius;
        float z = Mathf.Sin(angleRad) * ringRadius;
        return new Vector3(x, 0f, z);
    }

    private int GetRingIndexFromPosition(Vector3 position)
    {
        float distance = new Vector2(position.x, position.z).magnitude;
        float ringIndex = (distance - RingConstants.BaseRadius) / RingConstants.RingRadiusOffset;
        return Mathf.RoundToInt(ringIndex);
    }

    private int GetSegmentIndexFromPosition(Vector3 position)
    {
        float angle = Mathf.Atan2(position.z, position.x) * Mathf.Rad2Deg;
        return GetSegmentIndex(angle);
    }
}