using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder
{
    private LevelData levelData;
    private Dictionary<int, List<int>> validSegmentsPerRing;

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
            List<int> validSegments = new List<int>();

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


    private float GetSegmentAngle(int segmentIndex)
    {
        float angle = 90f - (segmentIndex * 15f);

        // Normalize to 0-360 range
        while (angle < 0) angle += 360f;
        while (angle >= 360f) angle -= 360f;

        return angle;
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

    public bool IsValidPosition(int ringIndex, float angle)
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

    public List<(int ringIndex, float angle)> GetValidAdjacentPositions(int currentRing, float currentAngle)
    {
        List<(int ringIndex, float angle)> adjacentPositions = new List<(int ringIndex, float angle)>();

        // Check adjacent angles on the same ring
        float[] adjacentAngles = { currentAngle - 15f, currentAngle + 15f };

        foreach (float angle in adjacentAngles)
        {
            if (IsValidPosition(currentRing, angle))
            {
                adjacentPositions.Add((currentRing, angle));
            }
        }

        // Check adjacent rings at the same angle (with bounds checking)
        int[] adjacentRings = { currentRing - 1, currentRing + 1 };

        foreach (int ringIndex in adjacentRings)
        {
            // Only check if the ring index is within valid bounds (0 to max ring)
            if (ringIndex >= 0 && ringIndex < levelData.rings.Count && IsValidPosition(ringIndex, currentAngle))
            {
                adjacentPositions.Add((ringIndex, currentAngle));
            }
        }

        return adjacentPositions;
    }

    

    public List<Waypoint> FindPath(int startRingIndex, int startSegmentIndex, int targetRingIndex, int targetSegmentIndex, Vector3 targetPosition)
    {

        float startAngle = GetSegmentAngle(startSegmentIndex);
        float targetAngle = GetSegmentAngle(targetSegmentIndex);
        
        List<PathNode> openSet = new List<PathNode>();
        List<PathNode> closedSet = new List<PathNode>();
        
        // Create start node
        PathNode startNode = new PathNode(startRingIndex, startAngle);
        startNode.gCost = 0;
        startNode.hCost = CalculateHeuristic(startNode, targetRingIndex, targetAngle);
        
        openSet.Add(startNode);
        
       
        
        // A* main loop
        while (openSet.Count > 0)
        {
            // Find node with lowest fCost
            PathNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost)
                {
                    currentNode = openSet[i];
                }
            }
            
            // Move current node from open to closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            
            // Check if we reached the target
            if (currentNode.ringIndex == targetRingIndex && Mathf.Abs(Mathf.DeltaAngle(currentNode.angle, targetAngle)) < 1f)
            {
                return BuildPath(currentNode, targetPosition);
            }
            
            // Explore neighbors
            var adjacentPositions = GetValidAdjacentPositions(currentNode.ringIndex, currentNode.angle);
            foreach (var adjacent in adjacentPositions)
            {
                // Skip if already in closed set
                if (closedSet.Any(node => node.ringIndex == adjacent.ringIndex && Mathf.Abs(Mathf.DeltaAngle(node.angle, adjacent.angle)) < 1f))
                    continue;
                
                
                // Calculate cost to this neighbor
                float moveCost = CalculateMoveCost(currentNode, adjacent);
                float tentativeGCost = currentNode.gCost + moveCost;
                
                // Check if this path is better than previous paths
                PathNode existingNeighbor = openSet.FirstOrDefault(node => 
                    node.ringIndex == adjacent.ringIndex && Mathf.Abs(Mathf.DeltaAngle(node.angle, adjacent.angle)) < 1f);
                
                if (existingNeighbor == null)
                {
                    // New node - add to open set
                    PathNode neighborNode = new PathNode(adjacent.ringIndex, adjacent.angle);
                    neighborNode.parent = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateHeuristic(neighborNode, targetRingIndex, targetAngle);
                    openSet.Add(neighborNode);
                }
                else if (tentativeGCost < existingNeighbor.gCost)
                {
                    // Better path found - update existing node
                    existingNeighbor.parent = currentNode;
                    existingNeighbor.gCost = tentativeGCost;
                }
            }
        }

        return new List<Waypoint>();
    }

    private List<Waypoint> BuildPath(PathNode endNode, Vector3 targetPosition)
    {
        List<Waypoint> path = new List<Waypoint>();
        PathNode currentNode = endNode;

        while (currentNode != null)
        {
            Vector3 position;

            if (path.Count == 0) {
                position = targetPosition;
            } else {
                position = GetPosition(currentNode.ringIndex, currentNode.angle);
            }

            path.Insert(0, new Waypoint(currentNode.ringIndex, GetSegmentIndex(currentNode.angle), position));
            currentNode = currentNode.parent;
        }

        return path;
    }

    private float CalculateHeuristic(PathNode node, int targetRing, float targetAngle)
    {
        // Simple heuristic: distance between rings + angular distance
        float ringDistance = Mathf.Abs(node.ringIndex - targetRing);
        float angleDistance = Mathf.Abs(Mathf.DeltaAngle(node.angle, targetAngle));

        return ringDistance + (angleDistance / 15f); // 15 degrees per segment
    }

    private float CalculateMoveCost(PathNode from, (int ringIndex, float angle) to)
    {
        // Cost for ring change
        float ringCost = Mathf.Abs(from.ringIndex - to.ringIndex);

        // Cost for angle change (normalized to segment count)
        float angleCost = Mathf.Abs(Mathf.DeltaAngle(from.angle, to.angle)) / 15f;

        return ringCost + angleCost;
    }


    private Vector3 GetPosition(int ringIndex, float angle)
    {
        float ringRadius = RingConstants.BaseRadius + ringIndex * RingConstants.RingSpacing;
        float angleRad = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleRad) * ringRadius;
        float z = Mathf.Sin(angleRad) * ringRadius;
        return new Vector3(x, 0f, z);
    }
}
