using Leopotam.EcsLite;

sealed class EnemiesMoveSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsPool<PlayerComponent> _player;
    private EcsFilter _playerFilter;

    private EcsPool<MainEnemyComponent> _enemies;
    private EcsFilter _enemiesFilter;

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        _playerFilter = world.Filter<PlayerComponent>().End();
        _player = world.GetPool<PlayerComponent>();
        
        _enemiesFilter = world.Filter<MainEnemyComponent>().Inc<LiveEnemyTag>().End();
        _enemies = world.GetPool<MainEnemyComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        //EcsWorld world = systems.GetWorld();

        //var playerFilter = world.Filter<PlayerComponent>().End();
        //var player = world.GetPool<PlayerComponent>();

        //var enemiesFilter = world.Filter<MainEnemyComponent>().Inc<LiveEnemyTag>().End();
        //var enemies = world.GetPool<MainEnemyComponent>();

        foreach (int playerID in _playerFilter)
        {
            ref var target = ref _player.Get(playerID);

            foreach (var enemyID in _enemiesFilter)
            {
                ref var enemy = ref _enemies.Get(enemyID);

                enemy.EnemyObject.transform.position = UnityEngine.Vector3.MoveTowards(enemy.EnemyObject.transform.position, target.Player.transform.position, 0.04f);
            }
        }
    }
}
