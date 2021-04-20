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

    bool preStart = false;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        preStart = true;
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
        player.SetNextCheckpoint(checkpoints[0].transform.position);
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
        else
        {
            checkpoints[currentCheckpoint].SetColor(currentCheckpointColor);
            player.SetNextCheckpoint(checkpoints[currentCheckpoint].transform.position);
        }
    }

    private void OnDisable()
    {
        foreach (Checkpoint c in checkpoints)
        {
            c.transform.gameObject.SetActive(false);
        }
        player.DisableCheckpoint();
    }

    private void OnEnable()
    {
        if (!preStart) return;

        if (player.IsMoveLocked()) return;

        ResetCourse();
        StartCoroutine(player.Teleport(startingLine.position, startingLine.rotation, true, OnPlayerReady));
    }

    private void OnPlayerReady()
    {
        //TODO: WAIT FOR INPUT
        player.ReleaseMoveLock();
        StartCourse();
    }
}
