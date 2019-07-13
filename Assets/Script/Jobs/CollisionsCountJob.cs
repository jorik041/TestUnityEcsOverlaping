using Script.Models;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Script.Jobs
{
    [BurstCompile]
    public struct CollisionsCountJob : IJob
    {
        [WriteOnly, DeallocateOnJobCompletion] public NativeList<CollisionData> CollisionData;

        public void Execute()
        {
            for (var i = 0; i < CollisionData.Length; i++)
            {
                // work
                //CollisionData[i];
            }
        }
    }
}