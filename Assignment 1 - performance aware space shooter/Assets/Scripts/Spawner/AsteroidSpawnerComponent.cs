using Unity.Entities;

public struct AsteroidSpawnerComponent : IComponentData
{
    public Entity asteroidPrefabToSpawn;
    public int numOfAsteroidsToSpawnPerSecond;
    public int numOfAsteroidsToSpawnIncrementAmount;
    public int maxNumberOfAsteroidsToSpawnPerSecond;
    public float asteroisSpawnRadius;
    public float minimumDistanceFromPlayer;

    public float timeBeforeNextSpawn;
    public float currentTimeBeforeNextSpawn;
}
