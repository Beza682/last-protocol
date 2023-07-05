using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesDictionary", menuName = "Enemies", order = 51)]
public class EnemiesDictionary : ScriptableObject
{
    [SerializeField] private EnemyInfo[] _enemies;
    private Dictionary<EnemiesEnum, BaseEnemyStateMachine> _enemiesMachines = new Dictionary<EnemiesEnum, BaseEnemyStateMachine>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEngine.Debug.Log($"Update Enemy Machines Dictionary");
        _enemiesMachines.Clear();

        foreach (var enemy in _enemies)
        {
            _enemiesMachines[enemy.EnemyType] = enemy.StateMachine;
        }
    }   
#endif

    internal EnemyInfo GetEnemy(in int enemyId)
    {
        return _enemies[enemyId];
    }
}

[Serializable]
public struct EnemyInfo
{
    public EnemiesEnum EnemyType;
    public GameObject Model;
    public BaseEnemy EnemyTrigger;
    public float Health;
    public float Speed;
    public BaseEnemyStateMachine StateMachine;
}