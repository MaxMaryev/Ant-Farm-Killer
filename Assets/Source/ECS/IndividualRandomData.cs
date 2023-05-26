using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Ants
{
    public struct IndividualRandomData : IComponentData
    {
        public Random Value;
    }
}
