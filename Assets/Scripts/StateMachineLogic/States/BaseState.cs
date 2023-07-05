public abstract class BaseState
{
    private readonly protected BaseEnemy Enemy;
    private readonly protected IStateSwitcher StateSwitcher;

    public BaseState(in BaseEnemy enemy, in IStateSwitcher state)
    {
        Enemy = enemy;
        StateSwitcher = state;
    }

    public abstract void Start();
    public abstract void Update();
    public abstract void Stop();
}
