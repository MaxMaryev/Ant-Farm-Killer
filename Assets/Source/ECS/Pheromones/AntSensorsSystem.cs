using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct AntSensorsSystem : ISystem
    {
        private float _time;
        private float _updateCooldownInSeconds;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _updateCooldownInSeconds = 0.1f;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _time += SystemAPI.Time.DeltaTime;

            if (_time < _updateCooldownInSeconds)
                return;
            else
                _time = 0;

            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var results = new NativeList<int>(Allocator.Temp);

            var physicsCollidersQuery = state.GetComponentLookup<Pheromone>(isReadOnly: false);
            
            foreach (var (sensor, transform) in SystemAPI.Query<RefRW<AntSensors>, LocalTransform>())
            {



                //int result =
                //    FindBodiesInAabbCube(physicsWorldSingleton.PhysicsWorld.CollisionWorld, CollisionFilter.Default, transform.Position, 100, ref results);
                //UnityEngine.Debug.Log(result);
            }

            //int FindBodiesInAabbCube(in CollisionWorld world, in CollisionFilter filter, in float3 pos, float range, ref NativeList<int> results)
            //{
            //    OverlapAabbInput overlapInput;
            //    overlapInput.Filter = filter;
            //    float3 radius3 = new float3(range, range, range);
            //    Aabb aabb;
            //    aabb.Max = pos + radius3;
            //    aabb.Min = pos - radius3;
            //    overlapInput.Aabb = aabb;
            //    world.OverlapAabb(overlapInput, ref results);
            //    return results.Length;
            //}


        }
    }

    public partial struct SensorJob : IJobEntity
    { 
        public void Execute()
        { 
        }
    }

}
