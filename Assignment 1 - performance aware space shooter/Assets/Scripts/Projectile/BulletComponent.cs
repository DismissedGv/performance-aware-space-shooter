using Unity.Entities;
using Unity.Mathematics;

public struct BulletComponent : IComponentData
{
    public float speed;
    public float size;
    public float damage;
}
