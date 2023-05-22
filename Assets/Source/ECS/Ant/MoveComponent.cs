using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.UIElements;
using UnityEngine;

public struct MoveComponent : IComponentData
{
    public float3 Position;
    public float3 Velocity;
    public float SteerStrenght;
    public float MaxSpeed;
    public float3 DesiredDirection;
    public float WanderStrenght;

    public void Move()
    {

    }
}
