using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseController : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    [SerializeField]
    Transform startingLine;
    [SerializeField]
    Checkpoint[] checkpoints;

    [SerializeField]
    Color previousCheckpointColor;
    [SerializeField]
    Color currentCheckpointColor;
    [SerializeField]
    Color nextCheckpointColor;

    int currentCheckpoint;
    bool inProgress;

    [SerializeField]
    bool debugNext = false;

    float countStartTime;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(inProgress)
        {
            if(checkpoints[currentCheckpoint].CheckTrigger(player.transform)) OnCheckpointReached();
        }
    }

    void ResetCourse()
    {
        inProgress = false;
        foreach (Checkpoint c in checkpoints)
        {
            c.SetColor(nextCheckpointColor);
            c.transform.gameObject.SetActive(true);
        }
    }

    void StartCourse()
    {
        checkpoints[0].SetColor(currentCheckpointColor);
        inProgress = true;
        currentCheckpoint = 0;
        countStartTime = Time.unscaledTime;
    }

    void OnCourseComplete()
    {
        float resultTime = Time.unscaledTime - countStartTime;
        inProgress = false;
        _ = DataOutput.Write(string.Format("{0}", TimeSpan.FromSeconds(resultTime)), "results.csv");
        OnDisable();
        this.gameObject.SetActive(false);
    }

    void OnCheckpointReached()
    {
        checkpoints[currentCheckpoint].SetColor(previousCheckpointColor);

        if (++currentCheckpoint >= checkpoints.Length) OnCourseComplete();
        else checkpoints[currentCheckpoint].SetColor(currentCheckpointColor);
    }

    private void OnDisable()
    {
        foreach (Checkpoint c in checkpoints)
        {
            c.transform.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ResetCourse();
        StartCoroutine(player.Teleport(startingLine.position, startingLine.rotation));
        //TODO: ADD TIMER TO START
        StartCourse();
    }
}
