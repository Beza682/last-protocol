public sealed class IdleState : BaseState
{
    public IdleState(in BaseEnemy enemy, in IStateSwitcher switcher) : base (enemy, switcher)
    {
        
    }

    public override void Start()
    {
        UnityEngine.Debug.Log($"Idle Start");
    }

    public override void Stop()
    {
        UnityEngine.Debug.Log($"Idle Stop");
    }

    public override void Update()
    {
        UnityEngine.Debug.Log($"Idle Update");
    }
}
