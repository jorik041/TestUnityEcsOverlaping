using Script.Components;
using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Script.Jobs
{
    [BurstCompile]
    [RequireComponentTag(typeof(TagAgentComponent))]
    public struct AABBTestJob : IJobForEachWithEntity<Translation2, AABBRadiusComponent>
    {
        [ReadOnly] public NativeMultiHashMap<int, QuadrantData2> QuadrantIndexToEntityMap;
        
        public void Execute(Entity entity, int index, [ReadOnly] ref Translation2 translation, [ReadOnly] ref AABBRadiusComponent aabbRadius)
        {
            var indexQuadrant = Utility.GetQuadrantHashByPosition(translation.Value, Utility.CellSize);

            if (QuadrantIndexToEntityMap.TryGetFirstValue(indexQuadrant, out var quadrantData, out var iterator))
            {
                do
                {
                    if (TestAABBAABB(ref translation, ref aabbRadius, ref quadrantData.Translation,
                        ref quadrantData.Radius))
                    {
                        // collision detected!
                    }
                } while (QuadrantIndexToEntityMap.TryGetNextValue(out quadrantData, ref iterator));
            }
        }
        
        bool TestAABBAABB(ref Translation2 center1, ref AABBRadiusComponent aabbRadius1, ref Translation2 center2, ref AABBRadiusComponent aabbRadius2)
        {
            uint r;
            r = (uint)(aabbRadius1.Value.x + aabbRadius2.Value.x);
            if ((uint)(center1.Value.x - center2.Value.x + r) > r + r) return false;
            
            r = (uint)(aabbRadius1.Value.y + aabbRadius2.Value.y);
            if ((uint)(center1.Value.y - center2.Value.y + r) > r + r) return false;
            return true;
        }
    }
}