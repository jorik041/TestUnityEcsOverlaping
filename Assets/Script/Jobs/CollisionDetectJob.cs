using Script.Components;
using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Script.Jobs
{
    [BurstCompile]
    [RequireComponentTag(typeof(TagAgentComponent))]
    public struct CollisionDetectJob : IJobForEachWithEntity<Translation, BoxColliderComponent>
    {
        [ReadOnly] public NativeMultiHashMap<int, QuadrantData> QuadrantIndexToEntityMap;
        //[WriteOnly] public NativeList<CollisionData> CollisionData;
        
        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref BoxColliderComponent boxCollider)
        {
            var indexQuadrant = Utility.GetQuadrantHashByPosition(translation.Value, Utility.CellSize);

            if (QuadrantIndexToEntityMap.TryGetFirstValue(indexQuadrant, out var quadrantData, out var iterator))
            {
                do
                {
                    if (translation.Value.x >= quadrantData.Position.x + quadrantData.BoxCollider.x)
                        continue;
                    if (translation.Value.x + boxCollider.Value.x <= quadrantData.Position.x)
                        continue;
                    if (translation.Value.y >= quadrantData.Position.y + quadrantData.BoxCollider.y)
                        continue;
                    if (translation.Value.y + boxCollider.Value.y <= quadrantData.Position.y)
                        continue;

                    // collision detected!
                    //CollisionData.Add(new CollisionData{Agent = entity, Obstacle = quadrantData.Entity});
                    return;
                } while (QuadrantIndexToEntityMap.TryGetNextValue(out quadrantData, ref iterator));
            }
        }
    }
}