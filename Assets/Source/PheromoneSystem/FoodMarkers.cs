using System.Collections.Generic;
using UnityEngine;

internal class FoodMarkers : PheromoneMap
{
    internal override IReadOnlyCollection<Pheromone> GetDetectedPheromonesCount(Vector3 position, Quaternion sensorDirection, Vector3 sensorHalfExtents)
    {

        Collider[] pheromoneColliders = Physics.OverlapBox(position, sensorHalfExtents, sensorDirection, Layers.FoodMarker);
        Pheromones = new List<Pheromone>();

        foreach (var collider in pheromoneColliders)
        {
            Pheromones.Add(collider.GetComponent<Pheromone>());
        }

        return Pheromones;
    }
}
