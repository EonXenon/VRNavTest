using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class TouchController : MonoBehaviour
{
    Dictionary<int, Vector2> lastTouchPositions;

    Vector2 rightHandLastTouchCenter;
    Vector2 rightHandLastTouchDelta = Vector2.zero;
    Vector2 leftHandLastTouchCenter;
    Vector2 leftHandLastTouchDelta = Vector2.zero;
    int rightHandLastTouchCount;
    int leftHandLastTouchCount;
    float rightHandLastDistance;
    float leftHandLastDistance;
    Dictionary<int, TouchControl> rightHandTouches;
    Dictionary<int, TouchControl> leftHandTouches;

    public Transform handInputUI;
    public Transform[] fingers;

    //public GameObject rightHandObj;
    //public GameObject leftHandObj;

    private bool TAP = false;
    private bool tapWait = false;
    private int tapCounter = 0;
    private DateTime tapTimeStamp;
    public float tapTimeSeconds = 0.3f;

    Vector2 centerCalibration = new Vector2(-960f, -380f);

    // Use this for initialization
    void Start()
    {
        lastTouchPositions = new Dictionary<int, Vector2>();

        rightHandLastTouchCount = 0;
        leftHandLastTouchCount = 0;
        rightHandTouches = new Dictionary<int, TouchControl>();
        leftHandTouches = new Dictionary<int, TouchControl>();
        handInputUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TAP = false;

        Dictionary<int, TouchControl> newRightHandTouches = new Dictionary<int, TouchControl>();
        Dictionary<int, TouchControl> newLeftHandTouches = new Dictionary<int, TouchControl>();

        if (Touchscreen.current == null) return;

        handInputUI.gameObject.SetActive(true);

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            TouchControl t = Touchscreen.current.touches[i];
            if (t.press.ReadValue() == 0f)
            {
                fingers[i].gameObject.SetActive(false);
                continue;
            }
            if (rightHandTouches.ContainsKey(t.touchId.ReadValue()))
            {
                newRightHandTouches[t.touchId.ReadValue()] = t;
            }
            else if (leftHandTouches.ContainsKey(t.touchId.ReadValue()))
            {
                newLeftHandTouches[t.touchId.ReadValue()] = t;
            }
            else if (t.position.ReadValue().x > (Screen.width / 2))
            {
                newRightHandTouches[t.touchId.ReadValue()] = t;
            }
            else
            {
                newLeftHandTouches[t.touchId.ReadValue()] = t;
            }
            Vector2 temp = t.position.ReadValue();
            fingers[i].localPosition = Vector3.up * (temp.y + centerCalibration.y) + Vector3.right * (temp.x + centerCalibration.x);
            fingers[i].gameObject.SetActive(true);
        }

        rightHandTouches = new Dictionary<int, TouchControl>(newRightHandTouches);
        leftHandTouches = new Dictionary<int, TouchControl>(newLeftHandTouches);

        List<TouchControl> rightHandTouchesList = rightHandTouches.Values.ToList();
        List<TouchControl> leftHandTouchesList = leftHandTouches.Values.ToList();

        // Right Hand

        if (rightHandTouchesList.Count > 0)
        {
            if (rightHandTouchesList.Count == 5)
            {
                if (!tapWait)
                {
                    tapWait = true;
                    tapTimeStamp = DateTime.Now;
                }
            }

            // Calc new center
            Vector2 touchCenter = calcCenter(rightHandTouchesList);

            // Calc avg touch distance to touch center
            //float avgDistance = calcAvgDistance(rightHandTouchesList, touchCenter);

            // Calc avg touch rotation around touch center
            //float avgRotation = calcAvgRotation(rightHandTouchesList, touchCenter, rightHandLastTouchCenter, rightHandLastTouchCount);

            // Calc scale and rotation
            //applyTransformations(rightHandObj, rightHandTouchesList.Count, rightHandLastTouchCount, touchCenter, rightHandLastTouchCenter, avgRotation, avgDistance, rightHandLastDistance);

            rightHandLastTouchDelta = calcAvgDelta(rightHandTouchesList);
            rightHandLastTouchCenter = touchCenter;
            //rightHandLastDistance = avgDistance;
            //rightHandObj.SetActive(true);
        }
        else
        {
            //rightHandObj.SetActive(false);
            rightHandLastTouchDelta = Vector2.zero;

            if (tapWait)
            {
                if (DateTime.Now < tapTimeStamp.AddMilliseconds(0.3f * 1000))
                {
                    TAP = true;
                }
                else
                {
                    tapWait = false;
                    tapCounter = 0;
                }
            }
        }

        

        rightHandLastTouchCount = rightHandTouchesList.Count;

        // Left Hand

        if (leftHandTouchesList.Count > 0)
        {
            // Calc new center
            Vector2 touchCenter = calcCenter(leftHandTouchesList);

            // Calc avg touch distance to touch center
            //float avgDistance = calcAvgDistance(leftHandTouchesList, touchCenter);

            // Calc avg touch rotation around touch center
            //float avgRotation = calcAvgRotation(leftHandTouchesList, touchCenter, leftHandLastTouchCenter, leftHandLastTouchCount);

            // Calc scale and rotation
            //applyTransformations(leftHandObj, leftHandTouchesList.Count, leftHandLastTouchCount, touchCenter, leftHandLastTouchCenter, avgRotation, avgDistance, leftHandLastDistance);


            leftHandLastTouchDelta = calcAvgDelta(leftHandTouchesList); ;
            leftHandLastTouchCenter = touchCenter;
            //leftHandLastDistance = avgDistance;
            //leftHandObj.SetActive(true);
        }
        else
        {
            //leftHandObj.SetActive(false);
            leftHandLastTouchDelta = Vector2.zero;
        }

        leftHandLastTouchCount = leftHandTouchesList.Count;

        // Update last touch positions

        lastTouchPositions.Clear();
        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            lastTouchPositions[Touchscreen.current.touches[i].touchId.ReadValue()] = Touchscreen.current.touches[i].position.ReadValue();
        }

    }

    public SimpleHandTouch GetHandInfo()
    {
        return new SimpleHandTouch(
                leftHandLastTouchDelta,
                leftHandLastTouchCount > 0,
                rightHandLastTouchDelta,
                rightHandLastTouchCount > 0
            );
    }

    private Vector2 calcCenter(List<TouchControl> touches)
    {
        Vector2 tMax = touches[0].position.ReadValue();
        Vector2 tMin = tMax;

        for (int i = 1; i < touches.Count; i++)
        {
            Vector2 tPos = touches[i].position.ReadValue();
            tMax = new Vector2(Mathf.Max(tMax.x, tPos.x), Mathf.Max(tMax.y, tPos.y));
            tMin = new Vector2(Mathf.Min(tMin.x, tPos.x), Mathf.Min(tMin.y, tPos.y));
        }

        return (tMin + tMax) / 2.0f;
    }

    private float calcAvgDistance(List<TouchControl> touches, Vector2 center)
    {
        float avgDistance = 0;
        for (int i = 0; i < touches.Count; i++)
        {
            avgDistance += (center - touches[i].position.ReadValue()).magnitude;
        }
        avgDistance /= (float)touches.Count;

        return avgDistance;
    }

    private Vector2 calcAvgDelta(List<TouchControl> touches)
    {
        Vector2 avgDelta = Vector2.zero;
        for (int i = 0; i < touches.Count; i++)
        {
            avgDelta += touches[i].delta.ReadValue();
        }
        avgDelta /= (float)touches.Count;

        return avgDelta;
    }

    private float calcAvgRotation(List<TouchControl> touches, Vector2 center, Vector2 lastCenter, int lastTouchCount)
    {
        float avgRotation = 0;
        if (lastTouchCount == touches.Count && touches.Count > 1)
        {
            for (int i = 0; i < touches.Count; i++)
            {
                Vector2 oldDir = lastTouchPositions[touches[i].touchId.ReadValue()] - lastCenter;
                Vector2 newDir = touches[i].position.ReadValue() - center;
                float angle = Vector2.Angle(oldDir, newDir);
                if (Vector3.Cross(oldDir, newDir).z < 0) angle = -angle;
                avgRotation += angle;
            }
            avgRotation /= (float)touches.Count;
        }

        return avgRotation;
    }

    /*private void applyTransformations(GameObject gameObject, int touchCount, int lastTouchCount, Vector2 touchCenter, Vector2 lastTouchCenter, float avgRotation, float avgDistance, float lastAvgDistance)
    {
        if (lastTouchCount == touchCount)
        {
            // scale

            if (touchCount > 1)
            {
                float scale = avgDistance / lastAvgDistance;
                gameObject.transform.localScale *= scale;
            }

            // rotate

            gameObject.transform.Rotate(0, 0, avgRotation);
        }

        if (lastTouchCount == 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.position = Vector3.zero;// new Vector3((touchCenter.x / (float)Screen.height - ((Screen.width / (float)Screen.height) / 2.0f)), touchCenter.y / (float)Screen.height - 0.5f, 0) * 2.0f;
        }
        else if (lastTouchCount == touchCount)
        {
            Vector2 touchCenterDelta = touchCenter - lastTouchCenter;
            gameObject.transform.position += new Vector3(touchCenterDelta.x / (float)Screen.height, touchCenterDelta.y / (float)Screen.height, 0) * 2.0f;
        }
    }*/
}

public class SimpleHandTouch
{
    public Vector2 leftHandDelta;
    public bool leftHandPress;
    public Vector2 rightHandDelta;
    public bool rightHandPress;

    public SimpleHandTouch(Vector2 leftHandDelta, bool leftHandPress, Vector2 rightHandDelta, bool rightHandPress)
    {
        this.leftHandDelta = leftHandDelta;
        this.leftHandPress = leftHandPress;
        this.rightHandDelta = rightHandDelta;
        this.rightHandPress = rightHandPress;
    }
}
