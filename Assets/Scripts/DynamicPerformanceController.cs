using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Rendering.HighDefinition;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DynamicPerformanceController : MonoBehaviour
{
    [Range(50f, 100f)]
    public float maximumResolutionPercentage = 100f;
    [Range(50f, 100f)]
    public float minimumResolutionPercentage = 50f;
    [Range(0f, 100f)]
    public float safetyMarginPercentage = 5f;

    [Range(50f, 5000f)]
    public float normalFarClip = 5000f;
    [Range(50f, 5000f)]
    public float maximumReducedFarClip = 500f;
    [Range(50f, 5000f)]
    public float minimumReducedFarClip = 50f;
    [Range(10f, 90f)]
    public float angleReducedFarClip = 50f;

    [Range(10f, 180f)]
    public float fieldOfViewOcclusionAngle = 50f;

    [Range(8, 128)]
    public int maximumSRRRays = 128;
    [Range(8, 128)]
    public int minimumSRRRays = 8;


    float resolutionScale = 100.0f;



    [SerializeField]
    Camera headCamera;
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
    float gpuTime = 0f;
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
        targetFramerate = Mathf.Max(DataInOut.config.targetFramerate,XRDevice.refreshRate);
        maximumResolutionPercentage = DataInOut.config.maximumResolutionPercentage;
        minimumResolutionPercentage = DataInOut.config.minimumResolutionPercentage;
        safetyMarginPercentage = DataInOut.config.safetyMarginPercentage;

        shortSmoothedCPU = smoothedCPU = shortSmoothedGPU = smoothedGPU = GetTargetTime();

        Application.targetFrameRate = 9999;

        DynamicResolutionHandler.SetDynamicResScaler(SetDynamicResolutionScale, DynamicResScalePolicyType.ReturnsPercentage);

        postProcess.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        postProcess.profile.TryGet<ScreenSpaceReflection>(out ssr);

        //ssr.quality.value = (int)ScalableSettingLevelParameter.Level.Medium;
        colorAdjustments.active = true;

        RenderPipelineManager.beginFrameRendering += StartWatch;
        RenderPipelineManager.endFrameRendering += StopWatch;
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

        float ratioError = Mathf.Min(smoothedRatio, finalRatio, (GetTargetTime() / (float)gpuTime) * 100f) - (100f - safetyMarginPercentage);

        /*if (ratioError < 0f)
        {
            resolutionScale = minimumResolutionPercentage;//Mathf.Max(resolutionScale - Time.deltaTime * (100f - minimumResolutionPercentage), minimumResolutionPercentage);
        }
        else if (ratioError > safetyMarginPercentage)
        {
            resolutionScale = maximumResolutionPercentage;//Mathf.Min(resolutionScale + Time.deltaTime * (100f - minimumResolutionPercentage), maximumResolutionPercentage);
        }*/

        if (gpuTime > GetTargetTime())
        {
            ssr.rayMaxIterations = minimumSRRRays;
            resolutionScale = minimumResolutionPercentage;
        }
        else
        {
            ssr.rayMaxIterations = maximumSRRRays;
            resolutionScale = maximumResolutionPercentage;
        }
            

        /*float totalAng = Mathf.Abs(headCamera.transform.rotation.eulerAngles.x - 90f);

        if (totalAng < angleReducedFarClip)
            headCamera.farClipPlane = Mathf.Lerp(minimumReducedFarClip, maximumReducedFarClip, Mathf.Pow(totalAng / angleReducedFarClip,3f));
        else
            headCamera.farClipPlane = normalFarClip;*/

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
            screenText.text = string.Format("Scale: {0:F1}%\nPerformance Target: {1:F1}%\nFrametime (total): {2:F2}ms\nFrametime (GPU): {3:F2}ms\nRender Distance: {4:F2}m\nQueued Frames: {5:F2}",
                resolutionScale,
                ratioError,
                smoothedCPU * 1000f,
                smoothedGPU * 1000f,
                headCamera.farClipPlane,
                gpuTimeQueue.Count);
    }

    Queue<double> gpuTimeQueue = new Queue<double>();

    void StartWatch(ScriptableRenderContext context, Camera[] cameras)
    {
        gpuTimeQueue.Enqueue(Time.realtimeSinceStartupAsDouble);
    }

    void StopWatch(ScriptableRenderContext context, Camera[] cameras)
    {
        gpuTime = (float)(Time.realtimeSinceStartupAsDouble - gpuTimeQueue.Dequeue());
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DynamicPerformanceController))]
    public class DPCEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DynamicPerformanceController myScript = (DynamicPerformanceController)target;

            if (GUILayout.Button("Select objects"))
            {
                
            }


        }
    }
#endif
}