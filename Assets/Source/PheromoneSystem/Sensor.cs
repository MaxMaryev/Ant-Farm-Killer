using UnityEngine;

public class Sensor
{
    private float _value;
    private Vector3 _halfExtents;
    private Vector3 _position;

    public Sensor(Vector3 halfExtents, Vector3 position)
    {
        _halfExtents = halfExtents;
        _position = position;
    }

    public float Value => _value;

    public void Update(Vector3 position, Vector3 direction, bool isSearchingFood)
    {
        _position = position + direction;
        _value = 0;

        PheromoneMap map = (isSearchingFood) ? new FoodMarkers() : new HomeMarkers();
        _value = map.GetDetectedPheromonesCount(_position, Quaternion.Euler(direction), _halfExtents);
    }
}