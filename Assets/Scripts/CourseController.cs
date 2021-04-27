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

    float countStartTime;

    bool preStart = false;
    int debugCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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

    void ReadyCourse()
    {
        checkpoints[0].SetColor(currentCheckpointColor);
        currentCheckpoint = 0;
        player.SetNextCheckpoint(checkpoints[0].transform.position, currentCheckpoint, checkpoints.Length);
    }

    void StartCourse()
    {
        inProgress = true;
        countStartTime = Time.unscaledTime;
    }

    void OnCourseComplete()
    {
        float resultTime = Time.unscaledTime - countStartTime;
        inProgress = false;
        _ = DataOutput.Write(string.Format("{0}", TimeSpan.FromSeconds(resultTime)), "results.csv");
        gameObject.SetActive(false);
    }

    void OnCheckpointReached()
    {
        checkpoints[currentCheckpoint].SetColor(previousCheckpointColor);
        

        if (++currentCheckpoint >= checkpoints.Length) OnCourseComplete();
        else
        {
            checkpoints[currentCheckpoint].SetColor(currentCheckpointColor);
            player.SetNextCheckpoint(checkpoints[currentCheckpoint].transform.position, currentCheckpoint, checkpoints.Length);
        }
    }

    private void OnDisable()
    {
        foreach (Checkpoint c in checkpoints)
        {
            c.transform.gameObject.SetActive(false);
        }
        player.DisableCheckpoint();
        player.ReleaseMoveLock();
    }

    private void OnEnable()
    {
        if (!preStart) return;

        if (player.IsMoveLocked())
        {
            gameObject.SetActive(false);
            return;
        }

        ResetCourse();
        player.StartCoroutine(player.Teleport(startingLine.position, startingLine.rotation, true, OnPlayerReady));
    }

    private void OnPlayerReady()
    {
        if (!gameObject.activeSelf)
        {
            player.ReleaseMoveLock();
            return;
        }
        ReadyCourse();
        StartCoroutine(WaitForPlayerInput());
    }

    IEnumerator WaitForPlayerInput()
    {
        while (!player.GetTranslationIntent())
            yield return null;

        player.ReleaseMoveLock();
        StartCourse();
    }
}
