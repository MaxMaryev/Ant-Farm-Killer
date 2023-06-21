using Unity.Entities;
using UnityEngine;

namespace ECS_Ants
{
    public class PheromonesSpawnAuthoring : MonoBehaviour
    {
        public GameObject PheromonePrefab;
        public float SpawnRate;
    }

    public class PheromonesSpawnBaker : Baker<PheromonesSpawnAuthoring>
    {
        public override void Bake(PheromonesSpawnAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new PheromonesSpawner
            {
                EntityPrefab = GetEntity(authoring.PheromonePrefab, TransformUsageFlags.None),
                SpawnRate = authoring.SpawnRate
            });
        }
    }
}
