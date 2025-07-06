public class PathNode
{
    public int ringIndex;
    public float angle;
    public float gCost; // Cost from start to this node
    public float hCost; // Heuristic cost from this node to target
    public PathNode parent;
    
    public float fCost => gCost + hCost; // Total cost
    
    public PathNode(int ringIndex, float angle)
    {
        this.ringIndex = ringIndex;
        this.angle = angle;
        this.gCost = float.MaxValue;
        this.hCost = 0f;
        this.parent = null;
    }
}