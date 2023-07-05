using UnityEngine;
using UnityEngine.UI;

public class JoystickSceneContext : MonoBehaviour
{
    [SerializeField] private Canvas _rootCanvas;
    public Canvas RootCanvas { get { return _rootCanvas; } }

    [SerializeField] private Image _stick;
    public Image Stick { get { return _stick; } }

    [SerializeField] private Image _back;
    public Image Back { get { return _back; } }

    [SerializeField] private GameObject _center;
    public GameObject Center { get { return _center; } }
}
