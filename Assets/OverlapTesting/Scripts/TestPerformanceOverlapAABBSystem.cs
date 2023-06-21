using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(TestPerformanceSystemGroup))]
public class TestPerformanceOverlapAABBSystem : SystemBase
{
    private const float boundingBoxExtents = 100f;

    private BuildPhysicsWorld buildPhysicsWorldSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        //Using overlap sphere
        CollisionWorld physicsWorld = buildPhysicsWorldSystem.PhysicsWorld.CollisionWorld;
        NativeList<int> outHits = new NativeList<int>(Allocator.TempJob);

        Entities
            .WithAll<TestOverlapSphereVsQuerySystemTag>()
            .WithReadOnly(physicsWorld)
            .WithDisposeOnCompletion(outHits)
            .ForEach((in LocalToWorld localToWorld) =>
            {
                CollisionFilter overlapSphereCollisionFilter = new CollisionFilter()
                {
                    BelongsTo = 1,
                    CollidesWith = 2,
                    GroupIndex = 0
                };

                var input = new OverlapAabbInput
                {
                    Aabb = new Aabb
                    {
                        Max = localToWorld.Position + new float3(boundingBoxExtents),
                        Min = localToWorld.Position - new float3(boundingBoxExtents)
                    },
                    Filter = overlapSphereCollisionFilter,
                };

                if (physicsWorld.OverlapAabb(input, ref outHits))
                {
                    for (int i = 0; i < outHits.Length; i++)
                    {
                        LocalToWorld l2w = GetComponent<LocalToWorld>(physicsWorld.Bodies[outHits[i]].Entity);

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