using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Ants
{
    public struct AntSensors : IComponentData
    {
        public float3 LeftSensorPosition; 
        public float3 CentralSensorPosition; 
        public float3 RightSensorPosition; 
        public int LeftSensorValue;
        public int CentralSensorValue;
        public int RightSensorValue;
    }
}
