using Unity.Entities;
using Unity.Physics;

namespace ECS_Ants
{
    public struct Pheromone : IComponentData
    {
        public PheromoneType PheromoneType;
        public int Value;
    }

    public enum PheromoneType
    {
        FoodMark,
        JustStepMark
    }
}
