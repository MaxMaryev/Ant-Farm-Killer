using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ECS_Ants
{
    public partial struct PheromonesSystem : ISystem
    {
        private float _time;
        private NativeList<Entity> _disappearedPheromones;
        private float _updateCooldownInSeconds;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _updateCooldownInSeconds = 1;
            _disappearedPheromones = new NativeList<Entity>(Allocator.Persistent);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            _disappearedPheromones.Dispose();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _time += SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            if (_time < _updateCooldownInSeconds)
                return;
            else
                _time = 0;

            foreach (var (pheromone, entity) in SystemAPI.Query<RefRW<PheromoneData>>().WithEntityAccess())
            {
                pheromone.ValueRW.Value -= 1;

                if (pheromone.ValueRO.Value < 0)
                {
                    _disappearedPheromones.Add(entity);
                }
            }

            UnityEngine.Debug.Log(_disappearedPheromones.Length);

            new DestroyPheromonesJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                Entities = _disappearedPheromones
            }.Run();

            _disappearedPheromones.Clear();
        }
    }

    [BurstCompile]
    public partial struct DestroyPheromonesJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public NativeList<Entity> Entities;

        private void Execute()
        {
            for (int i = 0; i < Entities.Length; i++)
            {
                ECB.DestroyEntity(Entities[i]);
            }
        }
    }
}
