using UnityEngine;

internal class Sensor
{
    internal Sensor(Vector3 halfExtents, Vector3 position)
    {
        HalfExtents = halfExtents;
        Position = position;
    }

    public Vector3 HalfExtents { get; internal set; }
    public float Value { get; internal set; }
    public Vector3 Position { get; internal set; }

    internal void UpdatePosition(Vector3 position, Vector3 direction)
    {
        Position = position + direction;
    }
}