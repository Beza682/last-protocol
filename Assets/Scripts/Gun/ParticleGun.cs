using System.Collections.Generic;
using UnityEngine;

public class ParticleGun : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
        int num = _particleSystem.GetCollisionEvents(other, events);

        for (int i = 0; i < num; i++)
        {
            if(other.TryGetComponent(out BaseEnemy enemyTrigger))
            {
                UnityEngine.Debug.Log($"Gun Attack");
                enemyTrigger.Death();
            }
        }    
    }
}
