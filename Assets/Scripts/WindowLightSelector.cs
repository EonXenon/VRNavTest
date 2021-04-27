using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WindowLightSelector : MonoBehaviour
{
    public Material litMaterial;
    public Material unlitMaterial;

    public void RandomizeLight()
    {
        MeshRenderer[] windows = Resources.FindObjectsOfTypeAll<MeshRenderer>();

        System.Random rng = new System.Random();

        foreach (MeshRenderer window in windows)
        {
            if (window.gameObject.GetComponent<WindowLightSelector>() != null)
                window.material = (rng.Next(0,2) == 1) ? litMaterial : unlitMaterial;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(WindowLightSelector))]
public class WindowLightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WindowLightSelector myScript = (WindowLightSelector)target;

        if(GUILayout.Button("Randomize Lit State"))
        {
            myScript.RandomizeLight();
        }
            

    }
}
#endif
