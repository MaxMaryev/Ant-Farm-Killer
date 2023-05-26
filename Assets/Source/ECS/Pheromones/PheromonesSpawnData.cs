using Unity.Entities;

public struct PheromonesSpawnData : IComponentData
{
    public Entity EntityPrefab;
    public float SpawnRate;
}
