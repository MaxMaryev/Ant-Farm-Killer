using UnityEngine;

public class Sensor
{
    private float _value;
    private ISensorable _sensorable;
    private Vector3 _position;
    private Vector3 _offset;
    private Vector3 _halfExtents;

    public Sensor(Vector3 halfExtents, Vector3 offset, ISensorable sensorable)
    {
        _halfExtents = halfExtents;
        _offset = offset;
        _sensorable = sensorable;
    }

    public float Value => _value;

    public void Update()
    {
        _position = _sensorable.RootTransform.TransformPoint(_offset);

        if (TryDetectCollision(Layers.Border))
        {
            _sensorable.OnBorderDetected();
        }
        else if (_sensorable.IsFoodDetecting && TryDetectCollision(Layers.Food))
        {
            _sensorable.OnFoodDetected();
            _value *= 100;
        }
        else if (_sensorable.IsHouseDetecting && TryDetectCollision(Layers.House))
        {
            _sensorable.OnHouseDetected();
            _value *= 100;
        }
        else
        {
            if (_sensorable.IsHouseDetecting)
                TryDetectCollision(Layers.Pheromone);
        }
    }

    private bool TryDetectCollision(int layer)
    {
        Collider[] detecteds =
            Physics.OverlapBox(_position, _halfExtents, Quaternion.Euler(_sensorable.RootTransform.forward), 1 << layer);

        _value = detecteds.Length;

        if (_value > 0)
        {
            return true;
        }
        else
            return false;
    }
}