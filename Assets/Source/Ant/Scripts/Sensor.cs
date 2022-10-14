using UnityEngine;

namespace Assets.Source.Ant
{
    internal class Sensor
    {
        public float Radius { get; internal set; }
        public float Value { get; internal set; }
        public Vector3 Position { get; internal set; }

        internal void UpdatePosition(Vector3 position, Vector3 direction)
        {
            Position = position;
        }
    }
}