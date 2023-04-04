using System.Collections.Generic;
using UnityEngine;

internal abstract class PheromoneMap
{
    protected List<Pheromone> Pheromones = new List<Pheromone>();

    internal PheromoneMap() { }

    internal abstract IReadOnlyCollection<Pheromone> GetDetectedPheromonesCount(Vector3 position, Quaternion sensorDirection, Vector3 areaHalfExtents);
}
