using System.Collections;
using System.Collections.Generic;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
using UnityEngine;

public class TranslationCourseRules : CourseRuleSet
{
    Vector3 lastPosition;
    public CourseController CC { get; set; }

    public override void ApplyRules()
    {
        CC = GetComponent<CourseController>();
        lastPosition = CC.StartingLine.position;
    }

    public override string ExecutionInfo(in PlayerController player) => player.GetCurrentTranslationMethod();

    public override float TakeMeasurement(in Transform player)
    {
        float distance = Vector3.Distance(player.position, lastPosition);
        lastPosition = player.position;
        return distance;
    }
}


#if (UNITY_EDITOR)
[CustomEditor(typeof(TranslationCourseRules))]
public class TranslationCourseRulesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TranslationCourseRules myScript = (TranslationCourseRules)target;

        myScript.CC = myScript.GetComponent<CourseController>();

        float totalDistance = 0f;
        float totalLateral = 0f;
        float totalForward = 0f;
        float totalVertical = 0f;

        Vector3 lastPoint = myScript.CC.StartingLine.position;

        for (int i = 0; i < myScript.CC.checkpoints.Length; i++)
        {
            Vector3 pos = myScript.CC.checkpoints[i].transform.position;
            totalDistance += Mathf.Abs(Vector3.Distance(pos,lastPoint));
            totalLateral += (Vector3.Scale((pos - lastPoint),myScript.CC.StartingLine.right)).magnitude;
            totalForward += (Vector3.Scale((pos - lastPoint),myScript.CC.StartingLine.forward)).magnitude;
            totalVertical += (Vector3.Scale((pos - lastPoint),myScript.CC.StartingLine.up)).magnitude;

            lastPoint = pos;
        }

        EditorGUILayout.LabelField("Total Distance:", totalDistance.ToString("F1"));
        EditorGUILayout.LabelField("Total Lateral Distance:", totalLateral.ToString("F1"));
        EditorGUILayout.LabelField("Total Forward Distance:", totalForward.ToString("F1"));
        EditorGUILayout.LabelField("Total Vertical Distance:", totalVertical.ToString("F1"));

    }

}

#endif