using System.Collections.Generic;
using UnityEngine;

internal class HomeMarkers : PheromoneMap
{
    internal override IReadOnlyCollection<Pheromone> GetDetectedPheromonesCount(Vector3 position, Quaternion sensorDirection, Vector3 sensorHalfExtents)
    {

        Collider[] pheromoneColliders = Physics.OverlapBox(position, sensorHalfExtents, sensorDirection, Layers.FoodMarker);
        List<Pheromone> pheromones = new List<Pheromone>();

        foreach (var collider in pheromoneColliders)
        {
            pheromones.Add(collider.GetComponent<Pheromone>());
        }

        return pheromones;
    }
}
