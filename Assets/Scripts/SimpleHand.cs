using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHand
{
    private static Vector2 frameSize = new Vector2(70.5f, 40.0f);

    private Dictionary<int, SimpleTouch> fingers;
    private Vector2 position;
    private Vector2 delta;
    private Vector2 accumulatedError;
    private bool tap;
    private bool touch;
    private bool revalidatePos = true;
    private bool revalidateDel = true;

    public SimpleHand()
    {
        fingers = new Dictionary<int, SimpleTouch>();
        accumulatedError = Vector2.zero;
    }

    public void Decay()
    {
        foreach (KeyValuePair<int, SimpleTouch> finger in fingers)
        {
            finger.Value.fresh = false;
        }
    }

    public void CullRipe()
    {
        List<int> marked = new List<int>();

        foreach (KeyValuePair<int, SimpleTouch> finger in fingers)
        {
            if (!finger.Value.fresh) marked.Add(finger.Key);
        }

        foreach (int key in marked)
        {
            RemoveFinger(key);
        }
    }

    public SimpleTouch this[int i]
    {
        get => fingers[i];

        set
        {
            fingers[i] = value;
            revalidatePos = true;
            revalidateDel = true;
        }
    }

    public Vector2 Position
    {
        get
        {
            if (revalidatePos)
            {
                if (fingers.Count <= 0) return Vector2.positiveInfinity;

                SimpleTouch[] touches = fingers.Values.ToArray();

                Vector2 tMax = touches[0].position;
                Vector2 tMin = tMax;

                for (int i = 1; i < touches.Length; i++)
                {
                    Vector2 tPos = touches[i].position;
                    tMax = new Vector2(Mathf.Max(tMax.x, tPos.x), Mathf.Max(tMax.y, tPos.y));
                    tMin = new Vector2(Mathf.Min(tMin.x, tPos.x), Mathf.Min(tMin.y, tPos.y));
                }
                position = (tMin + tMax) / 2.0f + accumulatedError;
                revalidatePos = false;
            }

            return position;
        }
    }

    public Vector2 Delta
    {
        get
        {
            if (revalidateDel)
            {
                if (fingers.Count <= 0) return Vector2.zero;

                SimpleTouch[] touches = fingers.Values.ToArray();

                delta = Vector2.zero;
                for (int i = 0; i < touches.Length; i++)
                {
                    delta += touches[i].delta;
                }
                delta /= (float)touches.Length;

                revalidateDel = false;
            }

            return delta;
        }
    }

    public bool Touch => touch;

    public int Count => fingers.Count;

    public bool Tap => tap;

    public bool ContainsFinger(int id) => fingers.ContainsKey(id);

    public void RemoveFinger(int id)
    {
        if (Count <= 1)
        {
            fingers.Remove(id);
            accumulatedError = Vector2.zero;
            revalidatePos = true;
            revalidateDel = true;
            return;
        }

        Vector2 cmp = Position;
        fingers.Remove(id);
        revalidatePos = true;
        revalidateDel = true;
        accumulatedError += cmp - Position;
        revalidatePos = true;
        revalidateDel = true;
    }

    public void Update(RectTransform handInputUI, Text[] blobs, int id)
    {
        foreach (KeyValuePair<int, SimpleTouch> finger in fingers)
        {
            blobs[finger.Value.i].transform.localPosition = Vector3.up * (finger.Value.position.y - 0.5f) * handInputUI.sizeDelta.y + Vector3.right * (finger.Value.position.x - 0.5f) * handInputUI.sizeDelta.x;
            blobs[finger.Value.i].gameObject.SetActive(true);
            blobs[finger.Value.i].text = id.ToString();
        }

        bool temp = Count > 0;
        tap = temp && !touch;
        touch = temp;
    }

    public float Distance(Vector2 position) => Vector2.Scale(position - Position, frameSize).magnitude;
}
