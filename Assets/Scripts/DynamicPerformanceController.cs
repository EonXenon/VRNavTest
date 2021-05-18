using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using System;

public class DynamicPerformanceController : MonoBehaviour
{

    public Volume postProcess;
    private ScreenSpaceReflection ssr;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 9999;
        postProcess.profile.TryGet<ScreenSpaceReflection>(out ssr);
        ssr.active = !DataInOut.config.disableSSR;
        ssr.rayMaxIterations = DataInOut.config.rayCount;
    }
}