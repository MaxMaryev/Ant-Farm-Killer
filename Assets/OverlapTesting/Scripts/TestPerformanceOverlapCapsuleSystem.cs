using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(TestPerformanceSystemGroup))]
public class TestPerformanceOverlapCapsuleSystem : SystemBase
{
    private const float capsuleRadius = 100f;

    private BuildPhysicsWorld buildPhysicsWorldSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        //Using overlap sphere
        CollisionWorld physicsWorld = buildPhysicsWorldSystem.PhysicsWorld.CollisionWorld;
        NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.TempJob);

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

                if (physicsWorld.OverlapCapsule(localToWorld.Position, localToWorld.Position + localToWorld.Forward * capsuleRadius * 2f, capsuleRadius, ref outHits, overlapSphereCollisionFilter))
                {
                    for (int i = 0; i < outHits.Length; i++)
                    {
                        LocalToWorld l2w = GetComponent<LocalToWorld>(outHits[i].Entity);

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