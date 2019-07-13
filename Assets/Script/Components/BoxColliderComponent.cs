using Unity.Entities;
using Unity.Mathematics;

namespace Script.Components
{
    public struct BoxColliderComponent: IComponentData
    {
        public float2 Value;
    
        public BoxColliderComponent(float2 value)
        {
            Value = value;
        }
    }
}