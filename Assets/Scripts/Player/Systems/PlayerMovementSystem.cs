using Leopotam.EcsLite;
using UnityEngine;

public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsPool<MainJoystickComponent> _joystickPool;
    private EcsPool<PlayerComponent> _playerPool;
    private EcsPool<OneFrameComponent> _oneFramePool;

    private EcsFilter _playerFilter;  
    private EcsFilter _joystickFilter;
    private EcsFilter _oneFrameFilter;


    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _joystickFilter = world.Filter<MainJoystickComponent>().Inc<EnableTag>().Exc<DisableTag>().End();
        _oneFrameFilter = world.Filter<MainJoystickComponent>().Inc<OneFrameComponent>().End();
        _playerFilter = world.Filter<PlayerComponent>().End();

        _joystickPool = world.GetPool<MainJoystickComponent>();
        _playerPool = world.GetPool<PlayerComponent>();
        _oneFramePool = world.GetPool<OneFrameComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        Move();
        Stop();
    }

    private void Move()
    {
        foreach (var joystick in _joystickFilter)
        {
            ref var joystickComponent = ref _joystickPool.Get(joystick);

            foreach (int player in _playerFilter)
            {
                ref var playerComponent = ref _playerPool.Get(player);

                playerComponent.Rigidbody.velocity = new Vector3(joystickComponent.Horizontal * playerComponent.Speed,
                                                           playerComponent.Rigidbody.velocity.y,
                                                           joystickComponent.Vertical * playerComponent.Speed);

                playerComponent.Animator.speed = playerComponent.Rigidbody.velocity.magnitude / 15;
                playerComponent.PlayerTransform.rotation = Quaternion.LookRotation(playerComponent.Rigidbody.velocity);
            }
        }
    }

    private void Stop()
    {
        foreach (var frame in _oneFrameFilter)
        {
            ref var joystickComponent = ref _joystickPool.Get(frame);

            foreach (int player in _playerFilter)
            {
                ref var playerComponent = ref _playerPool.Get(player);

                playerComponent.Rigidbody.velocity = Vector3.zero;
                playerComponent.Animator.speed = 0;    
            }

            _oneFramePool.Del(frame);
        }
    }
}