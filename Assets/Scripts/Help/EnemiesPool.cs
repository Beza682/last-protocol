using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Pool;

public class EnemiesPool
{
    private ObjectPool<BaseEnemy> _pool;

    public ObjectPool<BaseEnemy> Pool
    {
        get { return _pool; }
        private set { _pool = value; }
    }

    public EnemiesPool(EcsWorld world, EnemyInfo enemyInfo, int defaultCapacity, int maxSize, Transform containerParent)
    {
        GameObject container = new GameObject($"{enemyInfo.EnemyTrigger.gameObject.name}Pool");
        container.transform.SetParent(containerParent);

        _pool = new ObjectPool<BaseEnemy>(() => CreateEnemy(world, enemyInfo.EnemyTrigger, container.transform),
                                                      enemy => EnableEnemy(world, enemyInfo, enemy.Entity),
                                                      enemy => DisableEnemy(world, enemy.Entity),
                                                      enemy => DestroyEnemy(world, enemy.Entity),
                                                      false, defaultCapacity, maxSize);
    }

    private Vector3 FindPoint(Vector3 center, float angle, float radius)
    {
        return new Vector3(center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad),
                           center.y,
                           center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private BaseEnemy CreateEnemy(EcsWorld world, BaseEnemy prefab, Transform parent)
    {
        int entity = world.NewEntity();
        var enemiesMain = world.GetPool<MainEnemyComponent>();
        var enemiesPool = world.GetPool<EnemyPoolComponent>();
        var oneFramePool = world.GetPool<OneFrameComponent>();

        var playerFilter = world.Filter<PlayerComponent>().End();
        var playerPool = world.GetPool<PlayerComponent>();

        foreach (var player in playerFilter)
        {
            ref var component = ref playerPool.Get(player);

            BaseEnemy enemy = Object.Instantiate(prefab, component.Player.transform.position - Vector3.forward * 30,
                                                    Quaternion.identity, parent);

            enemy.Init(entity, () => _pool.Release(enemy));

            ref var main = ref enemiesMain.Add(entity);
            main.EnemyObject = enemy.gameObject;
            main.EnemyScript = enemy;

            enemiesPool.Add(entity).EnemiesPool = this;
            oneFramePool.Add(entity);

            return enemy;
        }

        return null;
    }

    private void EnableEnemy(EcsWorld world, EnemyInfo enemyInfo, int entity)
    {
        world.GetPool<LiveEnemyTag>().Add(entity);
        world.GetPool<DeadEnemyTag>().Del(entity);

        var playerFilter = world.Filter<PlayerComponent>().End();
        var player = world.GetPool<PlayerComponent>();

        foreach (int idx in playerFilter)
        {
            ref var component = ref player.Get(idx);

            ref var enemy = ref world.GetPool<MainEnemyComponent>().Get(entity);

            enemy.EnemyObject.transform.position = FindPoint(component.PlayerTransform.position, Random.Range(0, 360), 35);
            enemy.Health = enemyInfo.Health;
            enemy.EnemyObject.gameObject.SetActive(true);
        }
    }

    private void DisableEnemy(EcsWorld world, int entity)
    {
        world.GetPool<DeadEnemyTag>().Add(entity);
        world.GetPool<LiveEnemyTag>().Del(entity);

        world.GetPool<MainEnemyComponent>().Get(entity).EnemyObject.gameObject.SetActive(false);
    }

    private void DestroyEnemy(EcsWorld world, int entity)
    {
        UnityEngine.Debug.Log($"Clear Pool");
        ref var enemy = ref world.GetPool<MainEnemyComponent>().Get(entity);

        Object.Destroy(enemy.EnemyObject);
        world.DelEntity(entity);
    }
}