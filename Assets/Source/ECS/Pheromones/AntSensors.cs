using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Ants
{
    public struct AntSensors : IComponentData
    {
        public float3 _leftSensorPosition;
        public float3 _centralSensorPosition;
        public float3 _rightSensorPosition;
    }
}
