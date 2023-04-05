using UnityEngine;

public class Sensor
{
    private float _value;
    private Vector3 _halfExtents;
    private Vector3 _position;
    private Transform _headTransform;
    private int _detectedPheromones;

    public Sensor(Vector3 halfExtents, Vector3 position, Transform headTransform)
    {
        _halfExtents = halfExtents;
        _position = position;
        _headTransform = headTransform;
    }

    public float Value => _value;

    public void Update()
    {
        _position = _headTransform.position + _headTransform.forward;
        _value = 0;

        if (TryDetectCollision(Layers.Border))
        {
            _value = int.MinValue;
            Debug.Log("Border");
        }
        else if (TryDetectCollision(Layers.Pheromone))
        {
            _value = _detectedPheromones;
            Debug.Log("Pheromone");
        }
    }

    private bool TryDetectCollision(int layer)
    {
        Collider[] detecteds =
            Physics.OverlapBox(_position, _halfExtents, Quaternion.Euler(_headTransform.forward), layer);

        _detectedPheromones = detecteds.Length;

        if (_detectedPheromones > 0)
            return true;
        else
            return false;
    }
}