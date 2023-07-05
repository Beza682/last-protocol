using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using UnityEngine;
using UnityEngine.Scripting;

sealed class JoystickSystem : EcsUguiCallbackSystem, IEcsInitSystem
{
    private readonly EcsCustomInject<JoystickSceneContext> _joystickContext = default;
    private readonly EcsCustomInject<JoystickConfig> _joystickConfig = default;

    private EcsPool<MainJoystickComponent> _joystickPool;
    private EcsPool<EnableTag> _enablePool;
    private EcsPool<DisableTag> _disablePool;
    private EcsPool<OneFrameComponent> _oneFramePool;

    private EcsFilter _enableFilter;
    private EcsFilter _disableFilter;

    private Vector3 _deathArea;
    private int _lastId = -2;
    private float _diff;

    private int GetTouchID
    {
        get
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == _lastId)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    private float Radio => (_joystickConfig.Value.Radio * 10) + Mathf.Abs(_diff - _joystickContext.Value.Center.transform.position.magnitude);

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        _enableFilter = world.Filter<MainJoystickComponent>().Inc<EnableTag>().Exc<DisableTag>().End();
        _disableFilter = world.Filter<MainJoystickComponent>().Inc<DisableTag>().Exc<EnableTag>().End();

        _joystickPool = world.GetPool<MainJoystickComponent>();
        _enablePool = world.GetPool<EnableTag>();
        _disablePool = world.GetPool<DisableTag>();
        _oneFramePool = world.GetPool<OneFrameComponent>();

        _deathArea = _joystickContext.Value.Center.transform.position;
        _diff = _joystickContext.Value.Center.transform.position.magnitude;

        var entity = world.NewEntity();

        _joystickPool.Add(entity);
        _disablePool.Add(entity);
    }

    [Preserve, EcsUguiDragStartEvent("button_start-drag")]
    private void DragStart(in EcsUguiDragStartEvent evt)
    {
        if (_lastId == -2)
        {
            _lastId = evt.PointerId;

            evt.Sender.transform.localScale = Vector3.one * _joystickConfig.Value.OnPressScale;

            _joystickContext.Value.Back.CrossFadeColor(_joystickConfig.Value.PressColor,
                                                       _joystickConfig.Value.Duration,
                                                       false, true);
            _joystickContext.Value.Stick.CrossFadeColor(_joystickConfig.Value.PressColor,
                                                        _joystickConfig.Value.Duration,
                                                        false, true);

            foreach (var disable in _disableFilter)
            {
                _disablePool.Del(disable);
                _enablePool.Add(disable);
            }
        }
    }

    [Preserve, EcsUguiDragMoveEvent("button_move-drag")]
    private void DragMove(in EcsUguiDragMoveEvent evt)
    {
        if (evt.PointerId == _lastId)
        {
            foreach (var enable in _enableFilter)
            {
                Vector3 position = JoystickHelper.TouchPosition(_joystickContext.Value.RootCanvas, GetTouchID);
                
                if (Vector2.Distance(_deathArea, position) < Radio)
                    evt.Sender.transform.position = position;
                else
                    evt.Sender.transform.position = _deathArea + (position - _deathArea).normalized * Radio;

                ref var component = ref _joystickPool.Get(enable);

                component.Horizontal = (evt.Sender.transform.position.x - _deathArea.x) / Radio;
                component.Vertical = (evt.Sender.transform.position.y - _deathArea.y) / Radio;
            }
        }
    }

    [Preserve, EcsUguiDragEndEvent("button_end-drag")]
    private void DragEnd(ref EcsUguiDragEndEvent evt)
    {
        if (evt.PointerId == _lastId)
        {
            _lastId = -2;

            foreach (var enable in _enableFilter)
            {
                ref var component = ref _joystickPool.Get(enable);

                component.Horizontal = default;
                component.Vertical = default;

                _enablePool.Del(enable);
                _disablePool.Add(enable);
                _oneFramePool.Add(enable);
            }

            evt.Sender.transform.position = _deathArea;
            evt.Sender.transform.localScale = Vector3.one;

            _joystickContext.Value.Back.CrossFadeColor(_joystickConfig.Value.NormalColor,
                                                       _joystickConfig.Value.Duration,
                                                       false, true);
            _joystickContext.Value.Stick.CrossFadeColor(_joystickConfig.Value.NormalColor,
                                                        _joystickConfig.Value.Duration,
                                                        false, true);
        }
    }
}