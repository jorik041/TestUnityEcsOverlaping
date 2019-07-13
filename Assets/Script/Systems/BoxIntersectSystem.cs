using Script.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public class BoxIntersectSystem : JobComponentSystem
{
    private EntityQuery _obstacleQuery;

    protected override void OnCreate()
    {
        _obstacleQuery = GetEntityQuery(ComponentType.ReadOnly<Translation>(),
            ComponentType.ReadOnly<TagObstacleComponent>(),
            ComponentType.ReadOnly<BoxColliderComponent>());

    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new OverlapJob
        {
            ObstacleTranslationArray = _obstacleQuery.ToComponentDataArray<Translation>(Allocator.TempJob),
            ObstacleColliderArray = _obstacleQuery.ToComponentDataArray<BoxColliderComponent>(Allocator.TempJob)
        }.Schedule(this, inputDeps);

        return job;
    }

    [BurstCompile]
    [RequireComponentTag(typeof(TagAgentComponent))]
    private struct OverlapJob : IJobForEachWithEntity<Translation, BoxColliderComponent>
    {
        [ReadOnly, DeallocateOnJobCompletion] public NativeArray<Translation> ObstacleTranslationArray;
        [ReadOnly, DeallocateOnJobCompletion] public NativeArray<BoxColliderComponent> ObstacleColliderArray;

        public void Execute(Entity entity, int index, ref Translation translation, ref BoxColliderComponent boxCollider)
        {
            var length = ObstacleTranslationArray.Length;
            for (var i = 0; i < length; i++)
            {
                var obstacleTranslation = ObstacleTranslationArray[i];
                var obstacleCollider = ObstacleColliderArray[i];

                if (translation.Value.x >= obstacleTranslation.Value.x + obstacleCollider.Value.x)
                    continue;
                if (translation.Value.x + boxCollider.Value.x <= obstacleTranslation.Value.x)
                    continue;
                if (translation.Value.y >= obstacleTranslation.Value.y + obstacleCollider.Value.y)
                    continue;
                if (translation.Value.y + boxCollider.Value.y <= obstacleTranslation.Value.y)
                    continue;

                // collision detected!
                return;
            }
        }
    }
}