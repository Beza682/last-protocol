using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    [Range(1, 15)]
    public float Radio = 5;
    [Range(0.01f, 1)]
    public float SmoothTime = 0.5f;
    [Range(0.5f, 4)]
    public float OnPressScale = 1.5f;
    public Color NormalColor = new Color(1, 1, 1, 1);
    public Color PressColor = new Color(1, 1, 1, 1);
    [Range(0.1f, 5)] public float Duration = 1;

    [Header("Reference")]
    public RectTransform StickRect;
    public RectTransform CenterReference;

    private Vector3 DeathArea;
    private Vector3 currentVelocity;
    private bool isFree = false;
    private int lastId = -2;
    private Image stickImage;
    private Image backImage;
    private Canvas m_Canvas;
    private float diff;
    private Vector3 PressScaleVector;

    private int GetTouchID
    {
        get
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastId)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    private float radio => (Radio * 5) + Mathf.Abs(diff - CenterReference.position.magnitude);

    private float smoothTime => 1 - SmoothTime;

    public float Horizontal => (StickRect.position.x - DeathArea.x) / Radio;
    public float Vertical => (StickRect.position.y - DeathArea.y) / Radio;

    private void Start()
    {
        if (StickRect == null)
        {
            Debug.LogError("Please add the stick for joystick work!.");
            this.enabled = false;
            return;
        }

        if (transform.root.GetComponent<Canvas>() != null)
        {
            m_Canvas = transform.root.GetComponent<Canvas>();
        }
        else if (transform.root.GetComponentInChildren<Canvas>() != null)
        {
            m_Canvas = transform.root.GetComponentInChildren<Canvas>();
        }
        else
        {
            Debug.LogError("Required at lest one canvas for joystick work.!");
            this.enabled = false;
            return;
        }

        DeathArea = CenterReference.position;
        diff = CenterReference.position.magnitude;
        PressScaleVector = new Vector3(OnPressScale, OnPressScale, OnPressScale);

        if (GetComponent<Image>() != null)
        {
            backImage = GetComponent<Image>();
            stickImage = StickRect.GetComponent<Image>();
            backImage.CrossFadeColor(NormalColor, 0.1f, true, true);
            stickImage.CrossFadeColor(NormalColor, 0.1f, true, true);
        }
    }

    private void Update()
    {
        DeathArea = CenterReference.position;

        if (!isFree) return;

        StickRect.position = Vector3.SmoothDamp(StickRect.position, DeathArea, ref currentVelocity, smoothTime);

        if (Vector3.Distance(StickRect.position, DeathArea) < .1f)
        {
            isFree = false;
            StickRect.position = DeathArea;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (lastId == -2)
        {
            lastId = data.pointerId;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(true));
            OnDrag(data);
            if (backImage != null)
            {
                backImage.CrossFadeColor(PressColor, Duration, true, true);
                stickImage.CrossFadeColor(PressColor, Duration, true, true);
            }
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == lastId)
        {
            isFree = false;
            
            Vector3 position = JoystickHelper.TouchPosition(m_Canvas, GetTouchID);

            if (Vector2.Distance(DeathArea, position) < radio)
            {
                StickRect.position = position;
            }
            else
            {
                StickRect.position = DeathArea + (position - DeathArea).normalized * radio;
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        isFree = true;
        currentVelocity = Vector3.zero;
        
        if (data.pointerId == lastId)
        {
            lastId = -2;
            StopAllCoroutines();
            StartCoroutine(ScaleJoysctick(false));
            if (backImage != null)
            {
                backImage.CrossFadeColor(NormalColor, Duration, true, true);
                stickImage.CrossFadeColor(NormalColor, Duration, true, true);
            }
        }
    }

    private IEnumerator ScaleJoysctick(bool increase)
    {
        float _time = 0;

        while (_time < Duration)
        {
            Vector3 v = StickRect.localScale;
            if (increase)
            {
                v = Vector3.Lerp(StickRect.localScale, PressScaleVector, (_time / Duration));
            }
            else
            {
                v = Vector3.Lerp(StickRect.localScale, Vector3.one, (_time / Duration));
            }
            StickRect.localScale = v;
            _time += Time.deltaTime;
            yield return null;
        }
    }
}