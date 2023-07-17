using ECS_Ants;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public struct PheromonesList : IComponentData
{
    public NativeList<Pheromone> PheromoneDatas;
    public NativeList<float3> Positions;
}
