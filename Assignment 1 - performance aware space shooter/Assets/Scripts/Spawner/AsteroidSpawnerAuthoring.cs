using Unity.Entities;
using UnityEngine;

public class AsteroidSpawnerAuthoring : MonoBehaviour
{
    public GameObject asteroidPrefabToSpawn;
    public int numOfAsteroidsToSpawnPerSecond = 50;
    public int numOfAsteroidsToSpawnIncrementAmount = 2;
    public int maxNumberOfAsteroidsToSpawnPerSecond = 200;
    public float asteroisSpawnRadius = 40f;
    public float minimumDistanceFromPlayer = 5f;

    public float timeBeforeNextSpawn = 2f;

    public class AsteroidSpawnerBaker : Baker<AsteroidSpawnerAuthoring>
    {
        public override void Bake(AsteroidSpawnerAuthoring authoring)
        {
            Entity asteroidSpawnwerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(asteroidSpawnwerEntity, new AsteroidSpawnerComponent
            {
                asteroidPrefabToSpawn = GetEntity(authoring.asteroidPrefabToSpawn, TransformUsageFlags.None),
                numOfAsteroidsToSpawnPerSecond = authoring.numOfAsteroidsToSpawnPerSecond,
                numOfAsteroidsToSpawnIncrementAmount = authoring.numOfAsteroidsToSpawnIncrementAmount,
                maxNumberOfAsteroidsToSpawnPerSecond = authoring.maxNumberOfAsteroidsToSpawnPerSecond,
                asteroisSpawnRadius = authoring.asteroisSpawnRadius,
                minimumDistanceFromPlayer = authoring.minimumDistanceFromPlayer,
                timeBeforeNextSpawn = authoring.timeBeforeNextSpawn
            });
        }
    }
}
