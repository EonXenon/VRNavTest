using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;

public class DynamicResolutionTest : MonoBehaviour
{
    public float maxScale = 1.0f;
    public float minScale = 0.5f;
    public float scaleIncrement = 1f;
    public float deadzone = 0.05f;

    float resScale = 1.0f;
    private float target = 0f;
    private float smoothed = 0f;
    private float oldTime = 0f;

    private float currentFrameTime;
    private float lastDelta;

    public bool debugMode = false;
    public Text screenText;

    public float SetDynamicResolutionScale()
    {
        return resScale;
    }

    // Use this for initialization
    void Start()
    {
        target = XRDevice.refreshRate;
        if (target < 60) target = 120f;
        target = (1f / target);
        smoothed = target;
        Application.targetFrameRate = 9999;
        oldTime = Time.unscaledTime;
        DynamicResolutionHandler.SetDynamicResScaler(SetDynamicResolutionScale, DynamicResScalePolicyType.ReturnsMinMaxLerpFactor);

        currentFrameTime = Time.realtimeSinceStartup;
        lastDelta = Time.unscaledDeltaTime;
        StartCoroutine("WaitForNextFrame");
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            float newTime = Time.realtimeSinceStartup;
            lastDelta = currentFrameTime - newTime;
            currentFrameTime = newTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float curTime = Time.unscaledTime;
        float framerate = curTime - oldTime;
        oldTime = curTime;

        smoothed = (119f/120f) * smoothed + (1f/120f) * framerate;
        float ratio = target / smoothed;

        if (ratio > 1.0f - deadzone)
        {
            resScale = Mathf.Clamp(resScale + scaleIncrement * framerate, minScale, maxScale);
        }
        else if (ratio < 1.0f)
        {
            resScale = Mathf.Clamp(resScale - scaleIncrement * framerate, minScale, maxScale);
        }

        if (debugMode)
            screenText.text = string.Format("Scale: {0:F3}\nFrameTime: {1:F2}/{2:F2}/{3:F2}",
                resScale,
                smoothed * 1000f,
                target * 1000f,
                lastDelta * 1000f);
    }
}