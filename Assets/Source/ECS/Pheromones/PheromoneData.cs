using Unity.Entities;

namespace ECS_Ants
{
    public struct PheromoneData : IComponentData
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
