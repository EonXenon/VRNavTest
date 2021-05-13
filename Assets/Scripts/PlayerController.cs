using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform cameraHolder;
    private Rigidbody body;
    private CapsuleCollider coll;
    [SerializeField]
    private MeshRenderer fadeOut;
    private Material fadeOutEffect;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform trackingOffset;

    [SerializeField]
    private Transform checkpointGuide;
    private Vector3 nextCheckpoint;
    [SerializeField]
    private TextHolder checkpointText;
    [SerializeField]
    private TextHolder checkpointLabel;

    [SerializeField]
    private GameObject rotationAid;

    [SerializeField]
    private InputLayer inputLayer;


    [SerializeField]
    AudioClip startActSound;
    [SerializeField]
    AudioClip doActSound;
    [SerializeField]
    AudioClip endActSound;
    [SerializeField]
    AudioSource soundSource;

    private bool grounded = true;
    private bool movedLastFrame = true;
    private bool moveLocked = false;
    private bool rotateLocked = false;

    public float fadeTime = 0.5f;

    public float comfortVignetteStrength = 0.5f;

    public float speed = 5.0f;

    private float intendedHeight = 0f;

    void RecenterCamera()
    {
        trackingOffset.localEulerAngles = -head.localEulerAngles.y * Vector3.up;
        trackingOffset.localPosition = -head.localPosition.x * Vector3.right - head.localPosition.z * Vector3.forward + (intendedHeight - head.localPosition.y) * Vector3.up;
    }

    public void HookMonitor(MonitorHandler handler)
    {
        handler.HookInput(inputLayer);
    }

    void Start()
    {
        intendedHeight = 1.75f; //trackingOffset.localPosition.y; //TODO: get a better solution that isn't hardcoded

        body = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        fadeOutEffect = fadeOut.material;

        inputLayer.Initialize(this, RecenterCamera);

        SetFade(1f);
        moveLocked = rotateLocked = true;
        RecenterCamera();
        StartCoroutine(StartDelay(1f, fadeTime, RecenterCamera, ReleaseAllLock));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Resolve Input
        inputLayer.ResolveInput(in head, in cameraHolder);

        //Apply camera orientation to camera, NOT body!
        if (!rotateLocked)
            cameraHolder.localEulerAngles += inputLayer.GetCumulativeRotationInput();

        checkpointGuide.LookAt(nextCheckpoint, Vector3.up);

    }

    void FixedUpdate()
    {
        //Apply camera orientation to player orientation, and re-sync camera with object
        //This will mean that a player, like any object in the game, will update at the same fixed rate, so the behavior will be very predictable, while retaining a smooth visual behavior
        //Don't forget to set the Rigidbody to Interpolate!
        transform.Rotate(transform.up, cameraHolder.localEulerAngles.y);
        cameraHolder.localEulerAngles = Vector3.zero;

        Vector3 stop = -body.velocity;
        Vector3 move = inputLayer.GetCumulativeTranslationInput() * inputLayer.GetIntendedSpeedMultiplier(speed, speed * 5f);

        if (!moveLocked)
            body.AddForce(move + stop, ForceMode.VelocityChange);
        else
            body.AddForce(stop, ForceMode.VelocityChange);

        Vector3? movePosition = inputLayer.GetPositionalInput();

        if (movePosition != null)
        {
            body.isKinematic = true;
            body.MovePosition((Vector3)movePosition - (head.position - transform.position));
        }
        else
        {
            body.isKinematic = false;
        }

        movedLastFrame = stop.magnitude != 0f;
        grounded = Physics.Raycast(body.transform.position, -transform.up, coll.bounds.extents.y + 0.1f, ~(1 << 8));

    }

    static private Color blackZero = new Color(0f, 0f, 0f, 0f);
    static private Color blackOne = new Color(0f, 0f, 0f, 1f);

    public IEnumerator Teleport(Vector3 targetPosition, Quaternion targetRotation, bool keepOnHold = false, Action action = null)
    {
        if (moveLocked || rotateLocked) yield break;

        rotateLocked = moveLocked = true;

        float fade = 0f;
        while (fade < 1f)
        {
            fade = Mathf.Clamp01(fade + Time.unscaledDeltaTime / fadeTime);
            SetFade(fade);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.05f);
        transform.position = targetPosition;
        transform.localRotation = targetRotation;
        yield return new WaitForSecondsRealtime(0.05f);

        while (fade > 0f)
        {
            fade = Mathf.Clamp01(fade - Time.unscaledDeltaTime / fadeTime);
            SetFade(fade);
            yield return null;
        }

        if (!keepOnHold)
            rotateLocked = moveLocked = false;

        action?.Invoke();
    }

    public IEnumerator StartDelay(float waitTime, float fadeInTime, Action preAction = null, Action postAction = null)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        preAction?.Invoke();

        float fade = 1f;

        while (fade > 0f)
        {
            fade = Mathf.Clamp01(fade - Time.unscaledDeltaTime / fadeInTime);
            SetFade(fade);
            yield return null;
        }

        postAction?.Invoke();
    }

    public void ReleaseMoveLock() => moveLocked = false;

    public bool IsMoveLocked() => moveLocked;

    public void ReleaseRotateLock() => rotateLocked = false;

    public bool IsRotateLocked() => rotateLocked;

    public void ReleaseAllLock() => moveLocked = rotateLocked = false;

    public bool IsAnyLocked() => rotateLocked || moveLocked;

    public void SetNextCheckpoint(Vector3 targetPosition, int currentCheckpoint, int totalCheckpoints)
    {
        nextCheckpoint = targetPosition;
        checkpointGuide.gameObject.SetActive(true);
        checkpointText.text = String.Format("{0}/{1}", currentCheckpoint, totalCheckpoints);
        checkpointLabel.text = "Checkpoint";
        soundSource.PlayOneShot(doActSound);
    }

    public void SetCourseReady()
    {
        checkpointText.text = "MOVE TO START";
        checkpointLabel.text = "Awaiting input...";
    }

    public void StartActGeneric()
    {
        soundSource.PlayOneShot(startActSound);
    }

    public void DisableCheckpoint()
    {
        checkpointGuide.gameObject.SetActive(false);
        checkpointText.text = "";
        checkpointLabel.text = "";
        soundSource.PlayOneShot(endActSound);
    }

    public void SetFade(float perc) => fadeOutEffect.SetColor("_UnlitColor", Color.Lerp(blackZero, blackOne, perc));

    public bool GetTranslationIntent() => inputLayer.GetTranslationIntent();
    public bool GetRotationIntent() => inputLayer.GetRotationIntent();
    public bool GetAnyIntent() => inputLayer.GetRotationIntent() || inputLayer.GetTranslationIntent();

    public string GetCurrentRotationMethod() => inputLayer.rotationType.ToString();
    public string GetCurrentTranslationMethod() => inputLayer.translationType.ToString();

    public void EnableRotationAid() => rotationAid.SetActive(true);
    public void DisableRotationAid() => rotationAid.SetActive(false);
    public Transform GetHeadTransform() => head;

}
