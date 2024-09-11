using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }
    public void OnUpdate(ref SystemState state) 
    {
        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>()) 
        {
            if (spawner.ValueRO.NextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.prefab);

                float3 pos = new float3(spawner.ValueRO.spawnPos.x, spawner.ValueRO.spawnPos.y, 0);
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(pos));
                spawner.ValueRW.NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }
    }
}
