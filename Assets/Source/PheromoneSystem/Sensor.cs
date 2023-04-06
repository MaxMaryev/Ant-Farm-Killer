using System;
using UnityEngine;

public class Sensor
{
    private float _value;
    private Transform _antTransform;
    private Vector3 _position;
    private Vector3 _offset;
    private Vector3 _halfExtents;

    public Sensor(Vector3 halfExtents, Vector3 offset, Transform antTransform)
    {
        _halfExtents = halfExtents;
        _offset = offset;
        _antTransform = antTransform;
    }

    public float Value => _value;

    public void Update()
    {
        _value = 0;
        _position = _antTransform.TransformPoint(_offset);

        if (TryDetectCollision(Layers.Border))
        {
            _value = int.MinValue;
        }
        else
        {
            TryDetectCollision(Layers.Pheromone);
        }
    }

    private bool TryDetectCollision(int layer) 
    {
        Collider[] detecteds =
            Physics.OverlapBox(_position, _halfExtents, Quaternion.Euler(_antTransform.forward), 1 << layer);

        _value = detecteds.Length;

        if (_value > 0)
            return true;
        else
            return false;
    }
}