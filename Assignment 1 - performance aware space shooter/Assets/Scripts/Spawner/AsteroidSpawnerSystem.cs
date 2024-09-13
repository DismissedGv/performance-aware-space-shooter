using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;
using Unity.VisualScripting;

public partial struct AsteroidSpawnerSystem : ISystem
{
    private EntityManager _entitytManager;

    private Entity _astertoidSpawnerEntity;
    private AsteroidSpawnerComponent _asteroidSpawnerComponent;

    private Entity _playerEntity;
    
    private Unity.Mathematics.Random _random;

    private void OnCreate(ref SystemState state)
    {
        _random = Unity.Mathematics.Random.CreateFromIndex((uint)_asteroidSpawnerComponent.GetHashCode());
    }

    public void OnUpdate(ref SystemState state) 
    {
        _entitytManager = state.EntityManager;

        _astertoidSpawnerEntity = SystemAPI.GetSingletonEntity<AsteroidSpawnerComponent>();
        _asteroidSpawnerComponent = _entitytManager.GetComponentData<AsteroidSpawnerComponent>(_astertoidSpawnerEntity);

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        SpawnAsteroids(ref state);
    }

    private void SpawnAsteroids(ref SystemState state)
    {
        //decrement timer
        _asteroidSpawnerComponent.currentTimeBeforeNextSpawn -= SystemAPI.Time.DeltaTime;

        if (_asteroidSpawnerComponent.currentTimeBeforeNextSpawn <= 0)
        {
            for (int i = 0; i < _asteroidSpawnerComponent.numOfAsteroidsToSpawnPerSecond; i++)
            {
                EntityCommandBuffer ECB = new EntityCommandBuffer(Allocator.Temp);
                Entity asteroidEntity = _entitytManager.Instantiate(_asteroidSpawnerComponent.asteroidPrefabToSpawn);

                LocalTransform asteroidTransform = _entitytManager.GetComponentData<LocalTransform>(asteroidEntity);
                LocalTransform playerTransform = _entitytManager.GetComponentData<LocalTransform>(_playerEntity);

                //random spawn point
                float minDistanceSquared = _asteroidSpawnerComponent.minimumDistanceFromPlayer * _asteroidSpawnerComponent.minimumDistanceFromPlayer;
                float2 randomOffset = _random.NextFloat2Direction() * _random.NextFloat(_asteroidSpawnerComponent.minimumDistanceFromPlayer, _asteroidSpawnerComponent.asteroisSpawnRadius);
                float2 playerPosition = new float2(playerTransform.Position.x, playerTransform.Position.y);
                float2 spawnPosition = playerPosition + randomOffset;
                float distanceSquared = math.lengthsq(spawnPosition - playerPosition);

                if (distanceSquared < minDistanceSquared)
                {
                    spawnPosition = playerPosition + math.normalize(randomOffset) * math.sqrt(minDistanceSquared);
                }
                asteroidTransform.Position = new float3(spawnPosition.x, spawnPosition.y, 0f);

                //spawn look direction
                float3 direction = math.normalize(playerTransform.Position - asteroidTransform.Position);
                float angle = math.atan2(direction.y, direction.x);
                quaternion lookRot = quaternion.AxisAngle(new float3(0, 0, 1), angle);
                asteroidTransform.Rotation = lookRot;

                ECB.SetComponent(asteroidEntity, asteroidTransform);

                ECB.AddComponent(asteroidEntity, new AsteroidComponent
                {
                    currentHealth = 100f,
                    asteroidSpeed = 1.25f
                });

                ECB.Playback(_entitytManager);
                ECB.Dispose();
            }

            //increment the number of asteroids that spawn in each wave
            int desiredAsteroidsPerWave = _asteroidSpawnerComponent.numOfAsteroidsToSpawnPerSecond + _asteroidSpawnerComponent.numOfAsteroidsToSpawnIncrementAmount;
            int asteroidsPerWave = math.min(desiredAsteroidsPerWave, _asteroidSpawnerComponent.maxNumberOfAsteroidsToSpawnPerSecond);
            _asteroidSpawnerComponent.numOfAsteroidsToSpawnPerSecond = asteroidsPerWave;

            _asteroidSpawnerComponent.currentTimeBeforeNextSpawn = _asteroidSpawnerComponent.timeBeforeNextSpawn;
        }
    }
}
