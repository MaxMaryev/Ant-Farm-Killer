using System.Collections.Generic;
using UnityEngine;

public abstract class PheromoneMap
{
    public PheromoneMap() { }

    public abstract int GetDetectedPheromonesCount(Vector3 position, Quaternion sensorDirection, Vector3 areaHalfExtents);
}
