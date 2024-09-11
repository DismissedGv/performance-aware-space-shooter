using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private GameInput inputActions;
    private Entity player;
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }

        inputActions = new GameInput();
        inputActions.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveVector = inputActions.Player.Move.ReadValue<Vector2>();
        Vector2 mousePosition = inputActions.Player.MousePos.ReadValue<Vector2>();
        bool shoot = inputActions.Player.Shoot.IsPressed();

        SystemAPI.SetSingleton(new InputComponent
        {
            movement = moveVector,
            mousePosition = mousePosition,
            shoot = shoot
        });        
    }
}
