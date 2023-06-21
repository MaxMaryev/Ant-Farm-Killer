using Unity.Entities;

public struct PheromonesSpawner : IComponentData
{
    public Entity EntityPrefab;
    public float SpawnRate;
}
