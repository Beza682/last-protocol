using UnityEngine;

[CreateAssetMenu(fileName = "JoystickConfig", menuName = "Configs/Joystick", order = 51)]
public class JoystickConfig : ScriptableObject
{
    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] private float _radio = 5;
    [Range(0.01f, 1)]
    [SerializeField] private float _smoothTime = 0.5f;
    [Range(0.5f, 4)]
    [SerializeField] private  float _onPressScale = 1.5f;
    [SerializeField] private  Color _normalColor = new Color(1, 1, 1, 1);
    [SerializeField] private  Color _pressColor = new Color(1, 1, 1, 1);
    [Range(0.1f, 5)]
    [SerializeField] private  float _duration = 1;

    public float Radio { get { return _radio; } }
    public float SmoothTime { get { return _smoothTime; } }
    public float OnPressScale { get { return _onPressScale; } }
    public Color NormalColor { get { return _normalColor; } }
    public Color PressColor { get { return _pressColor; } }
    public float Duration { get { return _duration; } }
}
