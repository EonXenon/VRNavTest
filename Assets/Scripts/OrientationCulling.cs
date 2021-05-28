using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationCulling : MonoBehaviour
{
    public bool neverCull = false;
    public bool thicknessCompensation = false;
    public Vector3 faceOrientation;
    [HideInInspector]
    public Renderer objRenderer;
    [HideInInspector]
    public Renderer[] extraRenderers;

    void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        extraRenderers = GetComponentsInChildren<Renderer>();
    }
}
