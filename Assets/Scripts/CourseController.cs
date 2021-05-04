using System;
using System.Collections;
using UnityEngine;

public class CourseController : MonoBehaviour
{

    public enum CourseType
    {
        Hybrid,
        TranslationOnly,
        RotationOnly
    }

    [SerializeField]
    CourseType courseType;

    [SerializeField]
    string courseID;

    [SerializeField]
    PlayerController player;

    [SerializeField]
    Transform startingLine;
    [SerializeField]
    CourseObjective[] checkpoints;

    [SerializeField]
    Color previousCheckpointColor;
    [SerializeField]
    Color currentCheckpointColor;
    [SerializeField]
    Color nextCheckpointColor;

    CourseRuleSet rules;

    int currentCheckpoint;
    bool inProgress;

    float countStartTime;

    bool preStart = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rules = GetComponent<CourseRuleSet>();
    }

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
        foreach (CourseObjective c in checkpoints)
        {
            c.SetColor(nextCheckpointColor);
            c.transform.gameObject.SetActive(true);
        }
    }

    void ReadyCourse()
    {
        checkpoints[0].SetColor(currentCheckpointColor);
        currentCheckpoint = 0;
        player.SetCourseReady();
    }

    void StartCourse()
    {
        inProgress = true;
        countStartTime = Time.unscaledTime;
        player.SetNextCheckpoint(checkpoints[currentCheckpoint].transform.position, currentCheckpoint, checkpoints.Length);
    }

    void OnCourseComplete()
    {
        float resultTime = Time.unscaledTime - countStartTime;
        inProgress = false;
        _ = DataOutput.Write(String.Format("{0}", resultTime), "results_"+ courseID + ".csv");
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
        foreach (CourseObjective c in checkpoints)
        {
            c.transform.gameObject.SetActive(false);
        }

        if (!preStart) return;

        player.DisableCheckpoint();
        player.DisableRotationAid();
        player.ReleaseAllLock();
    }

    private void OnEnable()
    {
        if (!preStart) return;

        rules?.ApplyRules();

        if (player.IsAnyLocked())
        {
            preStart = true;
            gameObject.SetActive(false);
            preStart = false;
            return;
        }

        ResetCourse();
        player.StartActGeneric();
        player.StartCoroutine(player.Teleport(startingLine.position, startingLine.rotation, true, OnPlayerReady));
    }

    private void OnPlayerReady()
    {
        if (!gameObject.activeSelf)
        {
            player.ReleaseAllLock();
            return;
        }
        ReadyCourse();
        StartCoroutine(WaitForPlayerInput());
    }

    IEnumerator WaitForPlayerInput()
    {
        if (courseType == CourseType.TranslationOnly)
        {
            while (!player.GetTranslationIntent())
                yield return null;

            player.ReleaseMoveLock();
        }
        else if (courseType == CourseType.RotationOnly)
        {
            player.EnableRotationAid();

            while (!player.GetRotationIntent())
                yield return null;

            player.ReleaseRotateLock();
        }
        else
        {
            while (!player.GetAnyIntent())
                yield return null;

            player.ReleaseAllLock();
        }
        StartCourse();
    }
}
