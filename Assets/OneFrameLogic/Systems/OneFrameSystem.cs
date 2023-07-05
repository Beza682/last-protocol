using Leopotam.EcsLite;

sealed class OneFrameSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _oneFrameFilter;
    private EcsPool<OneFrameComponent> _oneFramePool;

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        _oneFrameFilter = world.Filter<OneFrameComponent>().End();
        _oneFramePool = world.GetPool<OneFrameComponent>();
    }

    public void Run (IEcsSystems systems) 
    {
        foreach (var idx in _oneFrameFilter)
        {
            _oneFramePool.Del(idx);
        }
    }
}
