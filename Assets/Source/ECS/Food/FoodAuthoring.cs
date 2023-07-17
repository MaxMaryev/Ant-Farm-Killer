using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS_Ants
{
    public class FoodAuthoring : MonoBehaviour
    {

    }

    public class FoodBaker : Baker<FoodAuthoring>
    {
        public override void Bake(FoodAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FoodTag());
        }
    }
}