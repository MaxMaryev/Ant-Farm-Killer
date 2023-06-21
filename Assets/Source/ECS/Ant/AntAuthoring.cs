using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS_Ants
{
    public class AntAuthoring : MonoBehaviour
    {
        public float3 Position;
        public float3 Velocity;
        public float SteerStrenght;
        public float MaxSpeed;
        public float3 DesiredDirection;
        public float WanderStrenght;
    }

    public class AntBaker : Baker<AntAuthoring>
    {
        public override void Bake(AntAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new MoveData
            {
                SteerStrenght = authoring.SteerStrenght,
                WanderStrenght = authoring.WanderStrenght,
                MaxSpeed = authoring.MaxSpeed
            });

            AddComponent<IndividualRandomData>(entity);
            AddComponent<AntSensors>(entity);
        }
    }
}
