using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

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
        foreach (var (transform, randomData, moveData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<IndividualRandomData>, RefRW<MoveComponent>>())
        {
            float3 randomDirection = randomData.ValueRW.Value.NextFloat3Direction();
            randomDirection.y = 0;
            moveData.ValueRW.DesiredDirection = math.normalize((moveData.ValueRO.DesiredDirection + randomDirection * moveData.ValueRO.WanderStrenght/* + _pheromoneSensors.GetDesiredDirection()*/));
            float3 desiredVelocity = moveData.ValueRO.DesiredDirection * moveData.ValueRO.MaxSpeed;
            float3 desiredSteeringForce = (desiredVelocity - moveData.ValueRW.Velocity) * moveData.ValueRO.SteerStrenght;
            float3 acceleration = ClampFloat3(desiredSteeringForce, moveData.ValueRO.SteerStrenght); //корень в ClampFloat3

            float3 velocityWithoutClamp = moveData.ValueRO.Velocity + acceleration * SystemAPI.Time.DeltaTime;
            moveData.ValueRW.Velocity = ClampFloat3(velocityWithoutClamp, moveData.ValueRO.MaxSpeed);
            moveData.ValueRW.Position += moveData.ValueRO.Velocity * SystemAPI.Time.DeltaTime;

            float angle = math.atan2(moveData.ValueRO.Velocity.x, moveData.ValueRO.Velocity.z);
            transform.ValueRW.Position = moveData.ValueRO.Position;
            transform.ValueRW.Rotation = quaternion.EulerZYX(0, angle, 0);
        }

        float3 ClampFloat3(in float3 vector, in float maxLength)
        {
            float sqrMagnitude = math.lengthsq(vector);
            float sqrLength = maxLength * maxLength;

            if (sqrMagnitude > sqrLength)
            {
                float3 normalized = vector / sqrMagnitude;
                return normalized * sqrLength;
            }

            return vector;
        }
    }

}
