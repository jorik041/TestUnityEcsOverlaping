using Unity.Entities;
using Unity.Mathematics;

namespace Script.Components
{
    public struct AABBRadiusComponent: IComponentData
    {
        public float2 Value;
    }
}