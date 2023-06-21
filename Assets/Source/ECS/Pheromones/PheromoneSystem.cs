using Unity.Burst;
using Unity.Entities;

namespace ECS_Ants
{
    public partial struct PheromoneSystem : ISystem
    {
        private float _time;
        private float _updateCooldownInSeconds;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _updateCooldownInSeconds = 1;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _time += SystemAPI.Time.DeltaTime;

            if (_time < _updateCooldownInSeconds)
                return;
            else
                _time = 0;

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            var setPheromoneValueJob = new SetPheromoneValueJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            };

            var destroyJob = new DestroyPheromonesJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            };

            setPheromoneValueJob.ScheduleParallel();
            destroyJob.ScheduleParallel(state.EntityManager.CreateEntityQuery(typeof(DisappearedPheromoneTag)));
        }
    }

    [BurstCompile]
    public partial struct DestroyPheromonesJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        private void Execute(Entity entity, [ChunkIndexInQuery] int sortKey)
        {
            ECB.DestroyEntity(sortKey, entity);
        }
    }

    [BurstCompile]
    public partial struct SetPheromoneValueJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        private void Execute(ref Pheromone pheromone, in Entity entity, [ChunkIndexInQuery] int sortKey)
        {
            pheromone.Value -= 1;
            
            if (pheromone.Value < 0)
            {
                ECB.SetComponentEnabled<DisappearedPheromoneTag>(sortKey, entity, true);
            }
        }
    }
}
