using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        GunAttack(world);
    }

    private void GunAttack(EcsWorld world)
    {
        var enemiesFilter = world.Filter<MainEnemyComponent>().Inc<LiveEnemyTag>().Exc<DeadEnemyTag>().End();
        var mainPool = world.GetPool<MainEnemyComponent>();
        var enemyPool = world.GetPool<EnemyPoolComponent>();

        var gunsFilter = world.Filter<GunComponent>().End();
        var gunsPool = world.GetPool<GunComponent>();

        foreach (var gunIdx in gunsFilter)
        {
            ref var gunComp = ref gunsPool.Get(gunIdx);
            List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();

            foreach (var enemyIdx in enemiesFilter)
            {
                ref var mainComp = ref mainPool.Get(enemyIdx);
                events.Clear();

                int enumerator = gunComp.GunParticle.GetCollisionEvents(mainComp.EnemyObject, events);

                for (int i = 0; i < enumerator; i++)
                {
                    mainComp.Health -= gunComp.GunDamage;
                    Debug.Log($"Damage {gunComp.GunDamage}");

                    if (mainComp.Health <= 0)
                    {
                        ref var enemyComp = ref enemyPool.Get(enemyIdx);
                        mainComp.EnemyScript.Death();
                    }
                }
            }
        }
    }
}
