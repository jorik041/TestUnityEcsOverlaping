using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Script.Jobs
{
    [BurstCompile]
    public struct AllocateCellsJob : IJob
    {
        public NativeMultiHashMap<int, QuadrantData> QuadrantIndexToEntityMap;

        [ReadOnly] public int CapacityWanted;

        public void Execute()
        {
            if (QuadrantIndexToEntityMap.Capacity < CapacityWanted)
            {
                QuadrantIndexToEntityMap.Capacity = CapacityWanted;
            }
        }
    }
}