using UnityEngine;

public class HomeMarkers : PheromoneMap
{
    public override int GetDetectedPheromonesCount(Vector3 position, Quaternion sensorDirection, Vector3 sensorHalfExtents)
    {
        Collider[] pheromones = Physics.OverlapBox(position, sensorHalfExtents, sensorDirection, Layers.FoodMarker);
        return pheromones.Length;
    }
}