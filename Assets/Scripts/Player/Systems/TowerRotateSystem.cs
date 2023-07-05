using Leopotam.EcsLite;
using System.ComponentModel;
using UnityEngine;

sealed class TowerRotateSystem : IEcsRunSystem
{
    Transform entity;
    float minDistance = float.MaxValue;

    public void Run(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        var playeFilter = world.Filter<PlayerComponent>().Inc<TowerComponent>().End();
        var playerPool = world.GetPool<PlayerComponent>();
        var towerPool = world.GetPool<TowerComponent>();

        var enemiesFilter = world.Filter<MainEnemyComponent>().Inc<LiveEnemyTag>().End();
        var mainPool = world.GetPool<MainEnemyComponent>();


        foreach (var playerIdx in playeFilter)
        {
            ref var playerComp = ref playerPool.Get(playerIdx);
            ref var towerComp = ref towerPool.Get(playerIdx);

            minDistance = float.MaxValue;

            foreach (var enemyIdx in enemiesFilter)
            {
                ref var mainComp = ref mainPool.Get(enemyIdx);

                float distance = Vector3.Distance(playerComp.PlayerTransform.position, mainComp.EnemyObject.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    entity = mainComp.EnemyObject.transform;

                }
            }

            if (enemiesFilter.GetEntitiesCount() > 0)
            {
                //towerComp.Tower.LookAt(entity);
                towerComp.Tower.rotation = Quaternion.LookRotation(entity.position - playerComp.PlayerTransform.position);

                //towerComp.Tower.LookAt(new Vector3(0, entity.position.y, 0));
            }
        }
    }
}
