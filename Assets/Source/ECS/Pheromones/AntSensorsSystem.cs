using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using System.Diagnostics;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct AntSensorsSystem : ISystem
    {
        private float _time;
        private float _updateCooldownInSeconds;
        private NativeList<float3> _pheromonePositions;
        private NativeList<Pheromone> _pheromoneDatas;
        private NativeList<float3> _foodPositions;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _updateCooldownInSeconds = 0.2f;
            _pheromonePositions = new NativeList<float3>(Allocator.Persistent);
            _pheromoneDatas = new NativeList<Pheromone>(Allocator.Persistent);
            _foodPositions = new NativeList<float3>(Allocator.Persistent);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            _pheromonePositions.Dispose();
            _pheromoneDatas.Dispose();
            _foodPositions.Dispose();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _time += SystemAPI.Time.DeltaTime;

            if (_time < _updateCooldownInSeconds)
                return;
            else
                _time = 0;

            _pheromonePositions.Clear();
            _pheromoneDatas.Clear();
            _foodPositions.Clear();

            foreach (var (pheromoneData, transform) in SystemAPI.Query<Pheromone, LocalTransform>())
            {
                _pheromonePositions.Add(transform.Position);
                _pheromoneDatas.Add(pheromoneData);
            }

            foreach (var (food, transform) in SystemAPI.Query<FoodTag, LocalTransform>())
                _foodPositions.Add(transform.Position);

            new SensorJob
            {
                PheromonePositions = _pheromonePositions,
                PheromoneDatas = _pheromoneDatas,
                FoodPositions = _foodPositions
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct SensorJob : IJobEntity
    {
        [ReadOnly] public NativeList<float3> PheromonePositions;
        [ReadOnly] public NativeList<Pheromone> PheromoneDatas;
        [ReadOnly] public NativeList<float3> FoodPositions;

        public void Execute(RefRW<AntSensors> antSensors, LocalTransform antTransform)
        {
            float3 leftSensorPosition = antTransform.Position + antTransform.TransformDirection(math.forward() * 0.6f + math.left() * 0.5f);
            float3 centralSensorPosition = antTransform.Position + antTransform.TransformDirection(math.forward() * 0.8f);
            float3 rightSensorPosition = antTransform.Position + antTransform.TransformDirection(math.forward() * 0.6f + math.right() * 0.5f);

            for (int i = 0; i < FoodPositions.Length; i++)
            {
                float3 foodPosition = FoodPositions[i];
                if (math.distancesq(foodPosition, centralSensorPosition) < 0.1f)
                {
                    antSensors.ValueRW.CentralSensorValue += 100;
                    return;
                }
                else if (math.distancesq(foodPosition, leftSensorPosition) < 0.1f)
                {
                    antSensors.ValueRW.LeftSensorValue += 100;
                    return;
                }
                else if (math.distancesq(foodPosition, rightSensorPosition) < 0.1f)
                {
                    antSensors.ValueRW.RightSensorValue += 100;
                    return;
                }
            }

            antSensors.ValueRW.LeftSensorValue = 0;
            antSensors.ValueRW.CentralSensorValue = 0;
            antSensors.ValueRW.RightSensorValue = 0;

            for (int i = 0; i < PheromoneDatas.Length; i++)
            {
                float3 pheromonePos = PheromonePositions[i];
                if (math.distancesq(pheromonePos, centralSensorPosition) < 0.1f)
                    antSensors.ValueRW.CentralSensorValue += 1;
                else if (math.distancesq(pheromonePos, leftSensorPosition) < 0.1f)
                    antSensors.ValueRW.LeftSensorValue += 1;
                else if (math.distancesq(pheromonePos, rightSensorPosition) < 0.1f)
                    antSensors.ValueRW.RightSensorValue += 1;
            }
        }
    }
}
