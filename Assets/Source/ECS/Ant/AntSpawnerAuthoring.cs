using Unity.Entities;
using UnityEngine;

namespace ECS_Ants
{
    public class AntSpawnerAuthoring : MonoBehaviour
    {
        public GameObject AntPrefab;
        public float SpawnRate;
    }

    public class AntSpawnerBaker : Baker<AntSpawnerAuthoring>
    {
        public override void Bake(AntSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(new AntSpawner
            {
                AntPrefab = GetEntity(authoring.AntPrefab, TransformUsageFlags.Dynamic),
                SpawnPosition = authoring.transform.position,
                NextSpawnTime = 0f,
                SpawnRate = authoring.SpawnRate
            });
        }
    }
}