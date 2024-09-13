using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;
using Unity.Burst;
using Unity.Physics;

[BurstCompile]
public partial struct BulletSystem : ISystem
{
    [BurstCompile]

    private void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifeTimeComponent>(entity))
            {
                //move bullet
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTransform.Position += bulletComponent.speed * SystemAPI.Time.DeltaTime * bulletTransform.Right();
                entityManager.SetComponentData(entity, bulletTransform);

                //decrement timer
                BulletLifeTimeComponent bulletLifeTimeComponent = entityManager.GetComponentData<BulletLifeTimeComponent>(entity);
                bulletLifeTimeComponent.remainingLifeTime -= SystemAPI.Time.DeltaTime;

                if (bulletLifeTimeComponent.remainingLifeTime <= 0)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }

                entityManager.SetComponentData(entity, bulletLifeTimeComponent);

                //physics
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                float3 point1 = new float3(bulletTransform.Position - bulletTransform.Right() * 0.15f);
                float3 point2 = new float3(bulletTransform.Position + bulletTransform.Right() * 0.15f);

                uint layerMask = LayerMaskHelper.GetLayerMaskFromTwoLayers(CollisionLayer.Wall, CollisionLayer.Enemy);

                physicsWorld.CapsuleCastAll(point1, point2, bulletComponent.size / 2, float3.zero, 1f, ref hits, new CollisionFilter
                {
                    BelongsTo = (uint)CollisionLayer.Default,
                    CollidesWith = layerMask
                });

                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        Entity hitEntity = hits[i].Entity;

                        if (entityManager.HasComponent<AsteroidComponent>(hitEntity))
                        {
                            AsteroidComponent asteroidComponent = entityManager.GetComponentData<AsteroidComponent>(hitEntity);
                            asteroidComponent.currentHealth -= bulletComponent.damage;
                            entityManager.SetComponentData(hitEntity, asteroidComponent);

                            if (asteroidComponent.currentHealth <= 0)
                            {
                                entityManager.DestroyEntity(hitEntity);
                            }
                        }

                    }

                    entityManager.DestroyEntity(entity);
                }

                hits.Dispose();
            }
        }
    }
   
}
