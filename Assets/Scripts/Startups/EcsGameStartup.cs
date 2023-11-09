using AB_Utility.FromSceneToEntityConverter;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;
using System.Collections.Generic;
using UnityEngine;

public sealed class EcsGameStartup : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;
    private EcsSystems _fixSystems;
    private EcsSystems _lateSystems;

    [SerializeField] private FPSCounterComtext _fpsCounterComtext;
    [SerializeField] private JoystickSceneContext _joystick;
    [SerializeField] private JoystickConfig _joystickConfig;
    [SerializeField] private Transform _containerNPC;
    [SerializeField] private EnemiesDictionary _enemiesDictionary;
    [SerializeField] private EcsUguiEmitter _emitter;

    public static Stack<EcsSystems> StackOfSystems = new Stack<EcsSystems>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _fixSystems = new EcsSystems(_world);
        _lateSystems = new EcsSystems(_world);

        AddSystems();
        AddFixSystems();
        AddLateSystems();

        AddInjections();

        InjectEmitter();

        _systems.ConvertScene().Init();
        _fixSystems.Init();
        _lateSystems.Init();

        StackOfSystems.Push(_systems);
    }

    private void FixedUpdate()
    {
        _fixSystems.Run();
    }

    private void Update()
    {
        foreach (var systems in StackOfSystems)
        {
            systems?.Run();
        }
    }

    private void LateUpdate()
    {
        _lateSystems.Run();
    }

    private void AddFixSystems()
    {
        _fixSystems
            .Add(new PlayerMovementSystem())
            ;
    }

    private void AddSystems()
    {
        _systems
            .Add(new EnemiesFabricSystem())
            .Add(new StateMachineSystem())
            .Add(new EnemiesMoveSystem())
            .Add(new JoystickSystem())
            //.Add(new TowerRotateSystem())
            .Add(new GunSystem())
            .Add(new FPSSystem())
            ;
    }

    private void AddLateSystems()
    {
        //_lateSystems
        //    .Add(new OneFrameSystem())
        //    ;
    }

    private void AddInjections()
    {
        //_fixSystems
        //    .Inject()
        //    ;
        _systems
            .Inject(_containerNPC)
            .Inject(_enemiesDictionary)
            .Inject(_fpsCounterComtext)
            .Inject(_joystick)
            .Inject(_joystickConfig)
            ;
        _lateSystems
            .Inject(_joystickConfig)
            ;
    }

    private void InjectEmitter()
    {
        _systems.InjectUgui(_emitter);
    }

    private bool _quit;

    private void OnApplicationFocus(bool focus)
    {
        _quit = focus;

        if (!focus)
        {

        }
        else
        {

        }
    }

    private void OnApplicationQuit()
    {
        if (_quit)
        {

        }
    }

    private void OnDestroy()
    {
        if (_systems == null) return;

        StackOfSystems.Clear();
        //_systems.GetWorld("ugui-events").Destroy();
        _systems.Destroy();
        _systems = null;        
        _fixSystems.Destroy();
        _fixSystems = null;
        _lateSystems.Destroy();
        _lateSystems = null;
        _world.Destroy();
        _world = null;
    }
}