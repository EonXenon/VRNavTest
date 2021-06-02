using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SimpleTouch
{
    public int id;
    public bool pressed;
    public Vector2 delta;
    public Vector2 position;

    public SimpleTouch(TouchControl t)
    {
        //Copy values
        this.id = t.touchId.ReadValue();
        this.pressed = t.press.ReadValue() != 0f;
        this.delta = t.delta.ReadUnprocessedValue();
        this.position = t.position.ReadUnprocessedValue();

        //Apply corrections
        this.delta.x /= Screen.height;
        this.delta.y /= Screen.height;
        this.position.x /= Screen.width;
        this.position.y /= Screen.height;
    }

    public static Vector2 CalculateHandPosition(SimpleTouch[] touches)
    {
        Vector2 tMax = touches[0].position;
        Vector2 tMin = tMax;

        for (int i = 1; i < touches.Length; i++)
        {
            Vector2 tPos = touches[i].position;
            tMax = new Vector2(Mathf.Max(tMax.x, tPos.x), Mathf.Max(tMax.y, tPos.y));
            tMin = new Vector2(Mathf.Min(tMin.x, tPos.x), Mathf.Min(tMin.y, tPos.y));
        }

        return (tMin + tMax) / 2.0f;
    }

    public static Vector2 CalculateHandDelta(SimpleTouch[] touches)
    {
        Vector2 avgDelta = Vector2.zero;
        for (int i = 0; i < touches.Length; i++)
        {
            avgDelta += touches[i].delta;
        }
        avgDelta /= (float)touches.Length;

        return avgDelta;
    }
}
