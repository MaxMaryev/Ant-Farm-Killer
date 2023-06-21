using Unity.Entities;

namespace ECS_Ants
{
    public struct AntSensors : IComponentData
    {
        public int _leftSensorValue;
        public int _centralSensorValue;
        public int _rightSensorValue;
    }
}
