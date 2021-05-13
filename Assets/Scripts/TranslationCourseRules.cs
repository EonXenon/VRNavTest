using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationCourseRules : CourseRuleSet
{
    [SerializeField]
    Transform startReference;
    Vector3 lastPosition;

    public override void ApplyRules() => lastPosition = startReference.position;

    public override string ExecutionInfo(in PlayerController player) => player.GetCurrentTranslationMethod();

    public override float TakeMeasurement(in Transform player)
    {
        float distance = Vector3.Distance(player.position, lastPosition);
        lastPosition = player.position;
        return distance;
    }
}
