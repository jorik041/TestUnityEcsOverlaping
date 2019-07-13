using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Script.Jobs
{
    [BurstCompile]
    public struct ClearCellsJob : IJob
    {
        [WriteOnly]
        public NativeMultiHashMap<int, QuadrantData> QuadrantIndexToEntityMap;

        public void Execute()
        {
            QuadrantIndexToEntityMap.Clear();
        }
    }
}