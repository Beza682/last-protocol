public sealed class SimpleEnemyStateMachine : BaseEnemyStateMachine
{
    public SimpleEnemyStateMachine(in BaseEnemy enemy, in StatesNPC defaultState) : base(defaultState)
    {
        StatesMap[StatesNPC.Idle] = new IdleState(enemy, this);
        StatesMap[StatesNPC.Stalk] = new StalkState(enemy, this);
        StatesMap[StatesNPC.Attack] = new AttackState(enemy, this);

        DefaultState();
    }
}
