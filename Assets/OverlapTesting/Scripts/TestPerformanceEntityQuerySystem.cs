using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(TestPerformanceSystemGroup))]
public class TestPerformanceEntityQuerySystem : SystemBase
{
    private const float testSphereRadius = 100f;

    private BuildPhysicsWorld buildPhysicsWorldSystem;
    private EntityQuery entityQuery;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
        entityQuery = GetEntityQuery(typeof(TestOverlapSphereVsQuerySystemTag));
    }

    protected override void OnUpdate()
    {
        //Using entity query
        PhysicsWorld physicsWorld = buildPhysicsWorldSystem.PhysicsWorld;
        NativeArray<Entity> entityQueryResults = entityQuery.ToEntityArray(Allocator.TempJob);

        Entities
            .WithAll<TestOverlapSphereVsQuerySystemTag>()
            .WithDisposeOnCompletion(entityQueryResults)
            .WithReadOnly(physicsWorld)
            .ForEach((in LocalToWorld localToWorld) =>
            {
                for (int i = 0; i < entityQueryResults.Length; i++)
                {
                    LocalToWorld l2w = GetComponent<LocalToWorld>(entityQueryResults[i]);
                    float sqrDist = Unity.Mathematics.math.lengthsq(l2w.Position - localToWorld.Position);

                    if (sqrDist < testSphereRadius * testSphereRadius)
                    {
                        CollisionFilter raycastCollisionFilter = new CollisionFilter()
                        {
                            BelongsTo = 1,
                            CollidesWith = 4,
                            GroupIndex = 0
                        };

                        RaycastInput raycastInput = new RaycastInput()
                        {
                            Start = localToWorld.Position,
                            End = l2w.Position,
                            Filter = raycastCollisionFilter
                        };

                        if (physicsWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
                        {

                        }
                    }
                }

            }).Run();
    }
}