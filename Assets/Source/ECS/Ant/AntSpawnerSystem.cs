using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

namespace ECS_Ants
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct AntSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<AntSpawner> spawner in SystemAPI.Query<RefRW<AntSpawner>>())
            {
                if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
                {
                    Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.AntPrefab);
                    state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
                    spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.SpawnRate;
                    SystemAPI.GetComponentRW<IndividualRandomData>(newEntity).ValueRW.Value.InitState((uint)newEntity.Index);
                }
            }
        }
    }
}
