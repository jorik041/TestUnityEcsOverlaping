using Unity.Entities;
using Unity.Mathematics;

namespace Script.Models
{
    public struct QuadrantData
    {
        public Entity Entity;
        public float3 Position;
        public float2 BoxCollider;
    }
}