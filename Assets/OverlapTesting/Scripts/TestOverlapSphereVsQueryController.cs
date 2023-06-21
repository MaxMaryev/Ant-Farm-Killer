using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class TestOverlapSphereVsQueryController : MonoBehaviour
{
    public GameObject TargetPrefab;
    public GameObject EnvironmentPrefab;

    public int TargetCount;
    public int EnvironmentCount;
    public float Radius;

    private Entity targetPrefabEntity;
    private Entity environmentPrefabEntity;

    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;

    private void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        targetPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(TargetPrefab, settings);
        environmentPrefabEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(EnvironmentPrefab, settings);
    }

    private void Start()
    {
        Unity.Mathematics.Random r = Unity.Mathematics.Random.CreateFromIndex(0);

        for (int i = 0; i < TargetCount; i++)
        {
            Entity targetEntity = entityManager.Instantiate(targetPrefabEntity);

            Translation randomPos = new Translation()
            {
                Value = (float3)transform.position + r.NextFloat3Direction() * r.NextFloat(0, Radius),
            };

            entityManager.AddComponentData(targetEntity, randomPos);
            entityManager.AddComponent<TestOverlapSphereVsQuerySystemTag>(targetEntity);
        }

        for (int i = 0; i < EnvironmentCount; i++)
        {
            Entity envEntity = entityManager.Instantiate(environmentPrefabEntity);

            Translation randomPos = new Translation()
            {
                Value = (float3)transform.position + r.NextFloat3Direction() * r.NextFloat(0, Radius),
            };

            entityManager.AddComponentData(envEntity, randomPos);
        }
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
}
