using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class FPSSystem: IEcsInitSystem, IEcsRunSystem
{
    private readonly EcsCustomInject<FPSCounterComtext> _fpsCounterContext;

    private FPSCounterComtext _fpsCounter;

    private byte _frameRange = 60;
    private byte[] _frameBuffer;
    private byte _frameBufferIndex = 0;

    public void Init(IEcsSystems systems)
    {
        _frameBuffer = new byte[_frameRange];
        _fpsCounter = Object.Instantiate(_fpsCounterContext.Value);
    }

    public void Run(IEcsSystems systems)
    {
        UpdateBuffer();
        CalculateFps();
    }

    private void UpdateBuffer()
    {
        _frameBuffer[_frameBufferIndex++] = (byte)(1f / Time.unscaledDeltaTime);

        if (_frameBufferIndex >= _frameRange) _frameBufferIndex = 0;
    }

    private void CalculateFps()
    {
        ushort sum = 0;

        for (byte i = 0; i < _frameRange; i++)
        {
            sum += _frameBuffer[i];
        }

        _fpsCounter.FPSCounter.text = $"{sum / _frameRange}";
    }
}
