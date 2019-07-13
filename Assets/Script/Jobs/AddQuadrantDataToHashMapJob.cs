using Script.Components;
using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Script.Jobs
{
    [BurstCompile]
    [RequireComponentTag(typeof(TagObstacleComponent))]
    public struct AddQuadrantDataToHashMapJob : IJobForEachWithEntity<Translation, BoxColliderComponent>
    {
        [WriteOnly] public NativeMultiHashMap<int, QuadrantData>.Concurrent QuadrantIndexToEntityMap;
        
        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref BoxColliderComponent boxCollider)
        {
            QuadrantIndexToEntityMap.Add(
                Utility.GetQuadrantHashByPosition(translation.Value, Utility.CellSize),
                new QuadrantData{Entity = entity, Position = translation.Value, BoxCollider = boxCollider.Value}
            );
        }
    }
}