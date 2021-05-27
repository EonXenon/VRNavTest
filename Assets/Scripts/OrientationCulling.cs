using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationCulling : MonoBehaviour
{
    public bool neverCull = false;
    public bool thicknessCompensation = false;
    public Vector3 faceOrientation;
    public Renderer objRenderer;

    void Awake()
    {
        objRenderer = GetComponent<Renderer>();
    }
}
