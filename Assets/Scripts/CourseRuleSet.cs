using UnityEngine;

public abstract class CourseRuleSet : MonoBehaviour
{
    public abstract void ApplyRules();
    public abstract float TakeMeasurement(in Transform player);
    public abstract string ExecutionInfo(in PlayerController player);
}
