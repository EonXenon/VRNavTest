using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Rendering.HighDefinition;

public class DynamicPerformanceController : MonoBehaviour
{
    [Range(50f, 100f)]
    public float maximumResolutionPercentage = 100f;
    [Range(50f, 100f)]
    public float minimumResolutionPercentage = 50f;
    [Range(0f, 100f)]
    public float safetyMarginPercentage = 5f;

    [Range(50f, 100f)]
    public float resolutionScale = 100.0f;

    public Volume postProcess;
    private ColorAdjustments colorAdjustments;
    private ScreenSpaceReflection ssr;

    float targetFramerate = 0f;
    float target = 0f;
    float smoothedCPU = 0f;
    float shortSmoothedCPU = 0f;
    float smoothedGPU = 0f;
    float shortSmoothedGPU = 0f;
    float smoothedRatio = 1f;
    float longSmoothedRatio = 1f;
    double gpuTime = 0f;
    bool askForTime = false;

    public bool debugMode = false;
    public Text screenText;

    public float SetDynamicResolutionScale()
    {
        return resolutionScale;
    }

    float GetTargetTime()
    {
        return (1f / targetFramerate) * ((100f - safetyMarginPercentage) / 100f);
    }

    // Use this for initialization
    void Start()
    {
        targetFramerate = Mathf.Max(80f,XRDevice.refreshRate);

        shortSmoothedCPU = smoothedCPU = shortSmoothedGPU = smoothedGPU = GetTargetTime();

        Application.targetFrameRate = 9999;

        DynamicResolutionHandler.SetDynamicResScaler(SetDynamicResolutionScale, DynamicResScalePolicyType.ReturnsPercentage);

        postProcess.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        postProcess.profile.TryGet<ScreenSpaceReflection>(out ssr);

        ssr.quality.value = (int)ScalableSettingLevelParameter.Level.Medium;
        colorAdjustments.active = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float framerate = Time.unscaledDeltaTime;

        smoothedCPU = ((targetFramerate - 1f) * smoothedCPU + framerate) / targetFramerate;
        shortSmoothedCPU = (((targetFramerate/10f) - 1f) * shortSmoothedCPU + framerate) / (targetFramerate / 10f);

        smoothedGPU = ((targetFramerate - 1f) * smoothedGPU + (float)gpuTime) / targetFramerate;
        shortSmoothedGPU = (((targetFramerate / 10f) - 1f) * shortSmoothedGPU + (float)gpuTime) / (targetFramerate / 10f);

        float extrapolated = 2f * shortSmoothedGPU - smoothedGPU;

        float finalRatio = (GetTargetTime() / extrapolated) * 100f;

        smoothedRatio = ((targetFramerate - 1f) * smoothedRatio + finalRatio) / targetFramerate;

        longSmoothedRatio = ((targetFramerate * 30f - 1f) * longSmoothedRatio + finalRatio) / (targetFramerate * 30f);

        float ratioError = Mathf.Min(smoothedRatio, finalRatio) - (100f - safetyMarginPercentage);

        if (ratioError < 0f)
        {
            resolutionScale = Mathf.Max(resolutionScale + ratioError, minimumResolutionPercentage);
        }
        else if (ratioError > safetyMarginPercentage)
        {
            resolutionScale = Mathf.Min(resolutionScale + Time.deltaTime * (100f - minimumResolutionPercentage), maximumResolutionPercentage);
        }

        /*if (resolutionScale <= minimumResolutionPercentage) //Performance too low, reduce quality further
        {
            ssr.quality.value = (int)ScalableSettingLevelParameter.Level.Low;
        }
        else if (resolutionScale >= (minimumResolutionPercentage + maximumResolutionPercentage)/2f)
        {
            if(longSmoothedRatio >= (100f - safetyMarginPercentage)) //Very good performance, max quality
                ssr.quality.value = (int)ScalableSettingLevelParameter.Level.High;
            else
                ssr.quality.value = (int)ScalableSettingLevelParameter.Level.Medium;

        }*/


        if (debugMode)
            screenText.text = string.Format("Scale: {0:F1}%\nPerformance Target: {1:F1}%\nFrametime (total): {2:F2}ms\nFrametime (GPU):{3:F2}ms",
                resolutionScale,
                ratioError,
                smoothedCPU * 1000f,
                smoothedGPU * 1000f);

        gpuTime = Time.realtimeSinceStartupAsDouble;
        askForTime = true;

    }

    void OnGUI()
    {
        if (askForTime)
        {
            gpuTime = Time.realtimeSinceStartupAsDouble - gpuTime;
            askForTime = false;
        }
    }
}