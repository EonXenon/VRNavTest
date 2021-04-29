using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCheckpoint : CourseObjective
{
    [SerializeField]
    LineRenderer line;
    Material lineMat;

    [Range(0, 1)]
    public float percentage;
    public float holdDuration = 2f;
    public float angleTolerance = 5f;

    float totalDuration;
    float[] segmentDuration;
    float[] cumulativeDuration;
    Vector3[] originalShape;

    [SerializeField]
    LineRenderer glyph;
    Material glyphMat;

    // Start is called before the first frame update
    void Start()
    {
        originalShape = new Vector3[line.positionCount];
        segmentDuration = new float[line.positionCount];
        cumulativeDuration = new float[line.positionCount];
        originalShape[0] = line.GetPosition(0);
        segmentDuration[0] = 0f;
        cumulativeDuration[0] = 0f;
        totalDuration = 0f;
        for (int i = 1; i < line.positionCount; i++)
        {
            originalShape[i] = line.GetPosition(i);
            segmentDuration[i] = (originalShape[i] - originalShape[i - 1]).magnitude;
            totalDuration += segmentDuration[i];
            cumulativeDuration[i] = totalDuration;
        }
        glyphMat = glyph.material;
        lineMat = line.material;

        percentage = 0f;
        UpdateState();
    }

    // Update is called once per frame
    void UpdateState()
    {
        float currentTime = totalDuration * percentage;

        int i = 1;
        while (cumulativeDuration[i] <= currentTime)
        {
            line.positionCount = Mathf.Max(i + 1, line.positionCount);
            line.SetPosition(i, originalShape[i]);
            if (++i >= originalShape.Length) return;
        }

        Vector3 curPos = Vector3.Lerp(originalShape[i], originalShape[i - 1], (cumulativeDuration[i] - currentTime) / segmentDuration[i]);
        line.positionCount = i + 1;
        line.SetPosition(i, curPos);   
    }

    public override bool CheckTrigger(Transform target)
    {
        float angle = Quaternion.FromToRotation(target.forward, transform.position - target.position).eulerAngles.y;
        if (angle > 180f) angle -= 360f;

        if (Mathf.Abs(angle) <= angleTolerance / 2f)
            percentage = Mathf.Min(1f, percentage + Time.deltaTime / holdDuration);
        else
            percentage = 0f;

        UpdateState();

        return percentage == 1f;
    }

    public override void SetColor(Color color)
    {
        if (glyphMat == null || lineMat == null)
        {
            lineMat = line.material;
            glyphMat = glyph.material;
        }
        glyphMat.SetColor("_EmissiveColor", color * 4096f);
        glyphMat.SetColor("_Color", color);
        lineMat.SetColor("_EmissiveColor", color * 4096f);
        lineMat.SetColor("_Color", color);
    }
}
