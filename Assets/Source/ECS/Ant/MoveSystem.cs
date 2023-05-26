using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using System.Linq;

namespace ECS_Ants
{
    [BurstCompile]
    public partial struct MoveSystem : ISystem
    {
        private float3 _randomDirection;
        private float3 _desiredVelocity;
        private float3 _desiredSteeringForce;
        private float3 _acceleration;
        private float3 _velocityWithoutClamp;
        private float _angle;

        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, randomData, moveData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<IndividualRandomData>, RefRW<MoveComponent>>())
            {
                _randomDirection = randomData.ValueRW.Value.NextFloat3Direction();
                _randomDirection.y = 0;
                moveData.ValueRW.DesiredDirection = math.normalize((moveData.ValueRO.DesiredDirection + _randomDirection * moveData.ValueRO.WanderStrenght/* + _pheromoneSensors.GetDesiredDirection()*/));
                _desiredVelocity = moveData.ValueRO.DesiredDirection * moveData.ValueRO.MaxSpeed;
                _desiredSteeringForce = (_desiredVelocity - moveData.ValueRW.Velocity) * moveData.ValueRO.SteerStrenght;
                _acceleration = MathUtils.ClampFloat3(_desiredSteeringForce, moveData.ValueRO.SteerStrenght);

                _velocityWithoutClamp = moveData.ValueRO.Velocity + _acceleration * SystemAPI.Time.DeltaTime;
                moveData.ValueRW.Velocity = MathUtils.ClampFloat3(_velocityWithoutClamp, moveData.ValueRO.MaxSpeed);
                moveData.ValueRW.Position += moveData.ValueRO.Velocity * SystemAPI.Time.DeltaTime;

                _angle = math.atan2(moveData.ValueRO.Velocity.x, moveData.ValueRO.Velocity.z);
                transform.ValueRW.Position = moveData.ValueRO.Position;
                transform.ValueRW.Rotation = quaternion.EulerZYX(0, _angle, 0);
            }
        }
    }
}
