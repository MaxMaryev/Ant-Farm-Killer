using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Ants
{
    public struct AntSpawner : IComponentData
    {
        public Entity AntPrefab;
        public float3 SpawnPosition;
        public float NextSpawnTime;
        public float SpawnRate;
    }
}
