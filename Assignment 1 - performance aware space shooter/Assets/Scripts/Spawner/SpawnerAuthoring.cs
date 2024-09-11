using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float spawnRate;

    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Spawner
            {
                prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                spawnPos = float2.zero,
                NextSpawnTime = 0,
                spawnRate = authoring.spawnRate
            });
        }
    }
}
