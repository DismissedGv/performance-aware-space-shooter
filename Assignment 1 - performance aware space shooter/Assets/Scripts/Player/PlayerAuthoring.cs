using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject projectilePrefab;

    class PlayerAuthorinBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(playerEntity, new PlayerComponent
            {
                moveSpeed = authoring.moveSpeed,
                projectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.None)
            });


        }
    }
}