using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

sealed class EnemiesFabricSystem : IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsCustomInject<Transform> _containerNPC = default;
    private readonly EcsCustomInject<EnemiesDictionary> _enemiesDictionary = default;

    private EnemiesPool _enemyPool;

    private float _timer = 2;

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        _enemyPool = new EnemiesPool(world, _enemiesDictionary.Value.GetEnemy(0), 100, 200, _containerNPC.Value.transform);
    }

    public void Run(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        SpawnSampleEnemies();
    }

    private void SpawnSampleEnemies()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _enemyPool.Pool.Get();

            _timer = 1.5f;
        }
    }

    private void DispawnEnemies(EcsWorld world)
    {
        var deadFilter = world.Filter<MainEnemyComponent>().Inc<DeadEnemyTag>().End();
        var mainPool = world.GetPool<MainEnemyComponent>();
        var deadPool = world.GetPool<DeadEnemyTag>();

        foreach (var deadIdx in deadFilter)
        {
            ref var deadComp = ref deadPool.Get(deadIdx);

        }
    }
}