using UnityEngine;

public static class JoystickHelper
{
    public static Vector3 TouchPosition(this Canvas canvas, int touchID)
    {
        Vector3 result = Vector3.zero;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
            result = Input.GetTouch(touchID).position;
#else
            result = Input.mousePosition;
#endif
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 tempVector = Vector2.zero;
#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
           Vector3 pos = Input.GetTouch(touchID).position;
#else
            Vector3 pos = Input.mousePosition;
#endif
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, canvas.worldCamera, out tempVector);
            result = canvas.transform.TransformPoint(tempVector);
        }

        return result;
    }
}