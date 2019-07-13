using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class MoveAgentsSystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem _commandBufferSystem;

    protected override void OnCreate()
    {
        _commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MoveJob
        {
            CommandBuffer = _commandBufferSystem.CreateCommandBuffer().ToConcurrent(),
            Random =  new Random((uint)UnityEngine.Random.Range(1, 100000))
        }.Schedule(this, inputDeps);
        
        _commandBufferSystem.AddJobHandleForProducer(job);

        return job;
    }
    
    [RequireComponentTag(typeof(TagAgentComponent))]
    private struct MoveJob : IJobForEachWithEntity<Translation>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public Random Random;
        
        public void Execute(Entity entity, int index, ref Translation translation)
        {
            var position = new float3(translation.Value.x + Random.NextFloat(-0.5f, 0.5f), translation.Value.y + Random.NextFloat(0.5f, -0.5f), 0);
            CommandBuffer.SetComponent(index, entity, new Translation {Value = position});
        }
    }
}