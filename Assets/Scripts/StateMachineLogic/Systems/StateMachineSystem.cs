using Leopotam.EcsLite;

public sealed class StateMachineSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _newEnemyFilter;
    private EcsFilter _enemyFilter;

    private EcsPool<MainEnemyComponent> _enemiesPool;    
    private EcsPool<StateMachineComponent> _stateMachinePool;
    private EcsPool<OneFrameComponent> _oneFramePool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _newEnemyFilter = world.Filter<MainEnemyComponent>().Inc<OneFrameComponent>().End();
        _enemyFilter = world.Filter<MainEnemyComponent>().Inc<StateMachineComponent>().End();

        _enemiesPool = world.GetPool<MainEnemyComponent>();
        _stateMachinePool = world.GetPool<StateMachineComponent>();
        _oneFramePool = world.GetPool<OneFrameComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        InitStateMachine();
        UpdateStateMachine();
    }

    private void InitStateMachine()
    {
        foreach (var newEnemy in _newEnemyFilter)
        {
            ref var enemyComponent = ref _enemiesPool.Get(newEnemy);
            ref var stateMachineComponent = ref _stateMachinePool.Add(newEnemy);

            var newStateMachine = new SimpleEnemyStateMachine(enemyComponent.EnemyScript, StatesNPC.Idle);

            stateMachineComponent.StateSwitcher = newStateMachine;
            stateMachineComponent.StateUpdate = newStateMachine;

            _oneFramePool.Del(newEnemy);
        }
    }

    private void UpdateStateMachine()
    {
        foreach (var enemy in _enemyFilter)
        {
            _stateMachinePool.Get(enemy).StateUpdate.StateUpdate();
        }
    }
}