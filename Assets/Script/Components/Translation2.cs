using Unity.Entities;
using Unity.Mathematics;

namespace Script.Components
{
    public struct Translation2 : IComponentData
    {
        public float2 Value;
    }
}