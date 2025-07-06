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
            Debug.Log($"Ring {ring.ringIndex}: {validSegments.Count} valid segments out of {ring.segments.Count}");
        }
    }

    public bool IsValidPosition(int ringIndex, float angle)
    {
        // Convert angle to segment index (matching the actual segment positioning)
        int segmentIndex = (6 - Mathf.RoundToInt(angle / 15f)) % 24;
        if (segmentIndex < 0) segmentIndex += 24;
        
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

    public List<(int ringIndex, float angle)> FindPath(int startRing, float startAngle, int targetRing, float targetAngle)
    {
        List<PathNode> openSet = new List<PathNode>();
        List<PathNode> closedSet = new List<PathNode>();
        
        // Create start node
        PathNode startNode = new PathNode(startRing, startAngle);
        startNode.gCost = 0;
        startNode.hCost = CalculateHeuristic(startNode, targetRing, targetAngle);
        
        openSet.Add(startNode);
        
        Debug.Log($"Starting A* pathfinding from ({startRing}, {startAngle}°) to ({targetRing}, {targetAngle}°)");
        
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
            
            Debug.Log($"Exploring node: Ring {currentNode.ringIndex}, Angle {currentNode.angle}°");
            
            // Check if we reached the target
            if (currentNode.ringIndex == targetRing && Mathf.Approximately(currentNode.angle, targetAngle))
            {
                Debug.Log("Target reached! Reconstructing path...");
                return ReconstructPath(currentNode);
            }
            
            // Explore neighbors
            var adjacentPositions = GetValidAdjacentPositions(currentNode.ringIndex, currentNode.angle);
            foreach (var adjacent in adjacentPositions)
            {
                // Skip if already in closed set
                if (closedSet.Any(node => node.ringIndex == adjacent.ringIndex && Mathf.Approximately(node.angle, adjacent.angle)))
                    continue;
                
                Debug.Log($"  Checking neighbor: Ring {adjacent.ringIndex}, Angle {adjacent.angle}°");
                
                // Calculate cost to this neighbor
                float moveCost = CalculateMoveCost(currentNode, adjacent);
                float tentativeGCost = currentNode.gCost + moveCost;
                
                // Check if this path is better than previous paths
                PathNode existingNeighbor = openSet.FirstOrDefault(node => 
                    node.ringIndex == adjacent.ringIndex && Mathf.Approximately(node.angle, adjacent.angle));
                
                if (existingNeighbor == null)
                {
                    // New node - add to open set
                    PathNode neighborNode = new PathNode(adjacent.ringIndex, adjacent.angle);
                    neighborNode.parent = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateHeuristic(neighborNode, targetRing, targetAngle);
                    openSet.Add(neighborNode);
                }
                else if (tentativeGCost < existingNeighbor.gCost)
                {
                    // Better path found - update existing node
                    existingNeighbor.parent = currentNode;
                    existingNeighbor.gCost = tentativeGCost;
                }
            }
            
            // For now, limit iterations to prevent infinite loop
            if (closedSet.Count > 100)
            {
                Debug.Log("Pathfinding stopped - too many iterations");
                break;
            }
        }
        
        Debug.Log("No path found!");
        return new List<(int ringIndex, float angle)>();
    }

    private List<(int ringIndex, float angle)> ReconstructPath(PathNode endNode)
    {
        List<(int ringIndex, float angle)> path = new List<(int ringIndex, float angle)>();
        PathNode currentNode = endNode;
        
        while (currentNode != null)
        {
            path.Insert(0, (currentNode.ringIndex, currentNode.angle));
            currentNode = currentNode.parent;
        }
        
        Debug.Log($"Path found with {path.Count} steps");
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
}
