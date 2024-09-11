using Unity.Entities;
using Unity.Mathematics;

public struct Spawner : IComponentData
{
    public Entity prefab;
    public float2 spawnPos; // better Vector2
    public float NextSpawnTime;
    public float spawnRate;
}
