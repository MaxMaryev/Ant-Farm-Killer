using Unity.Entities;
using Unity.Mathematics;

public struct AntSpawner : IComponentData
{
    public Entity AntPrefab;
    public float3 SpawnPosition;
    public float NextSpawnTime;
    public float SpawnRate;
}
