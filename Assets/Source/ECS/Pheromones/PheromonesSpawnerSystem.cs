using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct PheromonesSpawnerSystem : ISystem
    {
        private float _time;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawnData = SystemAPI.GetSingleton<PheromonesSpawner>();
            _time += SystemAPI.Time.DeltaTime;

            if (_time < spawnData.SpawnRate)
                return;
            else
                _time = 0;

            var ecbSingleton = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>();

            new PheromoneSpawnJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                Prefab = spawnData.EntityPrefab
            }.ScheduleParallel();
        }

        public partial struct PheromoneSpawnJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ECB;
            public Entity Prefab;

            public void Execute(in LocalTransform transform, in MoveData moveData, [ChunkIndexInQuery] int sortKey)
            {
                var newPheromone = ECB.Instantiate(sortKey, Prefab);
                ECB.SetComponent(sortKey, newPheromone, new LocalTransform { Position = transform.Position, Scale = 0.1f });
                ECB.AddComponent(sortKey, newPheromone, new Pheromone { Value = 20 });
                ECB.AddComponent<DisappearedPheromoneTag>(sortKey, newPheromone);
                ECB.SetComponentEnabled<DisappearedPheromoneTag>(sortKey, newPheromone, false);
            }
        }
    }
}
