using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCourseRules : CourseRuleSet
{
    [SerializeField]
    Transform startReference;
    [SerializeField]
    RotationCheckpoint[] checkpoints;
    [SerializeField]
    float[] rotations;

    public float distanceFromCenter = 15f;

    public override void ApplyRules()
    {
        if (rotations.Length != checkpoints.Length)
        {
            Debug.LogError("RotationCourseRules: Number of rotations does not match number of checkpoints!");
            return;
        }

        float curAng = 0;

        Shuffle(rotations);

        for (int i = 0; i < checkpoints.Length; i++)
        {
            curAng += rotations[i];
            checkpoints[i].transform.position = startReference.position + Quaternion.Euler(0f, curAng, 0f) * startReference.forward * distanceFromCenter - startReference.up * startReference.localPosition.y;
            checkpoints[i].transform.localRotation = Quaternion.Euler(0f, curAng, 0f);
        }

        lastOrientation = startReference.rotation;
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
