using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SimpleTouch
{
    public int id;
    public int i;
    public bool fresh;
    public bool pressed;
    public Vector2 delta;
    public Vector2 position;

    public SimpleTouch(TouchControl t, int i)
    {
        //Copy values
        this.id = t.touchId.ReadValue();
        this.pressed = t.press.ReadValue() != 0f;
        this.delta = t.delta.ReadUnprocessedValue();
        this.position = t.position.ReadUnprocessedValue();
        this.i = i;
        this.fresh = true;

        //Apply corrections
        this.delta.x /= Screen.width;
        this.delta.y /= Screen.height;
        this.position.x /= Screen.width;
        this.position.y /= Screen.height;
    }
}
