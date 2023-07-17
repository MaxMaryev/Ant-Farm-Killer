using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct MoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new MoveJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime,
                MapSizeX = 22,
                MapSizeZ = 22
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float DeltaTime;
        public float MapSizeX;
        public float MapSizeZ;

        [BurstCompile]
        public void Execute(RefRW<LocalTransform> transform, RefRW<IndividualRandomData> randomData, RefRW<MoveData> moveData, RefRO<AntSensors> antSensors)
        {
            {
                if (transform.ValueRO.Position.x > MapSizeX || transform.ValueRO.Position.x < -MapSizeX ||
                    transform.ValueRO.Position.z > MapSizeZ || transform.ValueRO.Position.z < -MapSizeZ)
                {
                    moveData.ValueRW.Velocity = -moveData.ValueRW.Velocity;
                }

                float wanderStrength = moveData.ValueRO.WanderStrenght;
                float steerStrength = moveData.ValueRO.SteerStrenght;
                float maxSpeed = moveData.ValueRO.MaxSpeed;
                float3 randomDirection = randomData.ValueRW.Value.NextFloat3Direction();
                randomDirection.y = 0;

                moveData.ValueRW.DesiredDirection = math.normalize(moveData.ValueRO.DesiredDirection + 
                    randomDirection * wanderStrength + transform.ValueRO.TransformDirection(GetPheromoneDirection(antSensors)));

                float3 desiredVelocity = moveData.ValueRO.DesiredDirection * maxSpeed;
                float3 desiredSteeringForce = (desiredVelocity - moveData.ValueRW.Velocity) * steerStrength;
                float3 acceleration = MathUtils.ClampFloat3(desiredSteeringForce, steerStrength);

                moveData.ValueRW.Velocity += acceleration * DeltaTime;
                moveData.ValueRW.Velocity = MathUtils.ClampFloat3(moveData.ValueRW.Velocity, maxSpeed);
                moveData.ValueRW.Position += moveData.ValueRW.Velocity * DeltaTime;

                float3 velocityWithoutY = moveData.ValueRW.Velocity;
                velocityWithoutY.y = 0;
                float3 forwardDir = math.normalize(velocityWithoutY);

                transform.ValueRW.Position = moveData.ValueRO.Position;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(forwardDir, math.up());
            }
        }

        [BurstCompile]
        private float3 GetPheromoneDirection(RefRO<AntSensors> antSensors)
        {
            if (antSensors.ValueRO.CentralSensorValue > math.max(antSensors.ValueRO.LeftSensorValue, antSensors.ValueRO.RightSensorValue))
                return math.forward();
            else if (antSensors.ValueRO.LeftSensorValue > antSensors.ValueRO.RightSensorValue)
                return math.left();
            else if (antSensors.ValueRO.RightSensorValue > antSensors.ValueRO.LeftSensorValue)
                return math.right();
            else
                return float3.zero;
        }
    }
}
