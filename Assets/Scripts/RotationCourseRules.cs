using System;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
using UnityEngine;

public class RotationCourseRules : CourseRuleSet
{
    public float[] rotations;
    public float distanceFromCenter = 15f;
    public CourseController CC { get; set; }

    public override void ApplyRules(in PlayerController player)
    {
        CC = GetComponent<CourseController>();
        Transform startReference = CC.StartingLine;

        if (rotations.Length != CC.checkpoints.Length)
        {
            Debug.LogError("RotationCourseRules: Number of rotations does not match number of checkpoints!");
            return;
        }

        float curAng = 0;

        Shuffle(rotations);

        for (int i = 0; i < CC.checkpoints.Length; i++)
        {
            curAng += rotations[i];
            CC.checkpoints[i].transform.position = startReference.position + Quaternion.Euler(0f, curAng, 0f) * startReference.forward * distanceFromCenter - startReference.up * startReference.localPosition.y;
            CC.checkpoints[i].transform.localRotation = Quaternion.Euler(0f, curAng, 0f);
        }

        lastOrientation = startReference.rotation;

        player.HideTranslationObjects();
    }

    Quaternion lastOrientation;

    public override float TakeMeasurement(in Transform player)
    {
        float angle = Quaternion.Angle(player.rotation, lastOrientation);
        lastOrientation = player.rotation;
        return angle;
    }

    private static System.Random rng = new System.Random();

    void Shuffle(float[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            float value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public override string ExecutionInfo(in PlayerController player) => player.GetCurrentRotationMethod();
}

#if (UNITY_EDITOR)
[CustomEditor(typeof(RotationCourseRules))]
public class RotationCourseRulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RotationCourseRules myScript = (RotationCourseRules)target;

        myScript.CC = myScript.GetComponent<CourseController>();

        if (myScript.rotations.Length != myScript.CC.checkpoints.Length)
            Array.Resize(ref myScript.rotations, myScript.CC.checkpoints.Length);

        float totalAng = 0f;
        foreach (float f in myScript.rotations)
        {
            totalAng += Mathf.Abs(f);
        }

        EditorGUILayout.LabelField("Total Angle:", totalAng.ToString("F1"));
            
    }

}
#endif