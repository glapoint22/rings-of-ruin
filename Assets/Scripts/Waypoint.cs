using System.Collections.Generic;
using UnityEngine;

public struct Waypoint
{
    public WaypointType waypointType;
    public Vector3 position;

    public Waypoint(WaypointType waypointType, Vector3 position)
    {
        this.waypointType = waypointType;
        this.position = position;
    }
}