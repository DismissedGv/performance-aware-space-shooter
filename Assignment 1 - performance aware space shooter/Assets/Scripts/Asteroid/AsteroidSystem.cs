using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct AsteroidSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity playerEntity;

    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;

        playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(playerEntity);

        NativeArray<Entity> allEntities = _entityManager.GetAllEntities();
        foreach (Entity entity in allEntities)
        {
            if (_entityManager.HasComponent<AsteroidComponent>(entity))
            {
                //move asteroid
                LocalTransform asteroidTransform = _entityManager.GetComponentData<LocalTransform>(entity);
                AsteroidComponent _asteroidComponent = _entityManager.GetComponentData<AsteroidComponent>(entity);
                float3 moveDirection = math.normalize(playerTransform.Position - asteroidTransform.Position); // asteroidTransform.forward;

                asteroidTransform.Position += _asteroidComponent.asteroidSpeed * SystemAPI.Time.DeltaTime * moveDirection;

                //look at player
                float3 lookDirection = math.normalize(playerTransform.Position - asteroidTransform.Position);
                float angle = math.atan2(lookDirection.y, lookDirection.x);
                angle -= math.radians(90f);
                quaternion lookRot = quaternion.AxisAngle(new float3(0, 0, 1), angle);
                asteroidTransform.Rotation = lookRot;

                _entityManager.SetComponentData(entity, asteroidTransform);


                _entityManager.SetComponentData(entity, asteroidTransform);
            }
        }
    }
}
