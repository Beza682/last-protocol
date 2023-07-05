using System;

public interface IEnemy
{
    public int Entity { get; }

    public void Init(int entity, Action action);
}
