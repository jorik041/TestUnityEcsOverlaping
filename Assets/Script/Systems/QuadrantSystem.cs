using Script.Jobs;
using Script.Models;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Script.Systems
{
    //[DisableAutoCreation]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class QuadrantSystem : JobComponentSystem
    {
        private EntityQuery _translationQuery;
        private NativeMultiHashMap<int, QuadrantData> _quadrantIndexToEntityMap;

        protected override void OnCreate()
        {
            _translationQuery = GetEntityQuery(ComponentType.ReadOnly<Translation>());
            _quadrantIndexToEntityMap = new NativeMultiHashMap<int, QuadrantData>(1, Allocator.Persistent);
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            //var collisionData = new NativeList<CollisionData>(Allocator.TempJob);
            
            var allocateCellsJobHandle = new AllocateCellsJob
            {
                QuadrantIndexToEntityMap = _quadrantIndexToEntityMap,
                CapacityWanted = _translationQuery.CalculateLength()
            }.Schedule(inputDeps);

            var addQuadrantDataToHashMapJobHandle = new AddQuadrantDataToHashMapJob
            {
                QuadrantIndexToEntityMap = _quadrantIndexToEntityMap.ToConcurrent()
            }.Schedule(this, allocateCellsJobHandle);
            
            var collisionDetectJobHandle = new CollisionDetectJob
            {
                QuadrantIndexToEntityMap = _quadrantIndexToEntityMap,
                //CollisionData = collisionData
            }.Schedule(this, addQuadrantDataToHashMapJobHandle);
            
            var clearCellsJobHandle = new ClearCellsJob
            {
                QuadrantIndexToEntityMap = _quadrantIndexToEntityMap
            }.Schedule(collisionDetectJobHandle);
            
            /*var collisionsCountJobHandle = new CollisionsCountJob
            {
                CollisionData = collisionData
            }.Schedule(collisionDetectJobHandle);
            
            return JobHandle.CombineDependencies(clearCellsJobHandle, collisionsCountJobHandle);*/
            return clearCellsJobHandle;
        }

        protected override void OnDestroy()
        {
            _quadrantIndexToEntityMap.Dispose();
        }
    }
}