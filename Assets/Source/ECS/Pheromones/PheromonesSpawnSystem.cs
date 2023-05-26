using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct PheromonesSpawnSystem : ISystem
    {
        private float _time;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spawnData = SystemAPI.GetSingleton<PheromonesSpawnData>();
            _time += SystemAPI.Time.DeltaTime;

            if (_time < spawnData.SpawnRate)
                return;
            else
                _time = 0;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, moveData) in SystemAPI.Query<LocalTransform, MoveComponent>())
            {
                var newPheromone = ecb.Instantiate(spawnData.EntityPrefab);
                ecb.SetComponent(newPheromone, new LocalTransform { Position = transform.Position, Scale = 0.1f });
                ecb.AddComponent(newPheromone, new PheromoneData { Value = 20 });
            }

            ecb.Playback(state.EntityManager);
        }
    }
}
