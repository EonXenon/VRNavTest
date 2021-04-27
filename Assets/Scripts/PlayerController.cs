using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
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
    private TouchController touch;

    [SerializeField]
    private InputLayer inputLayer;

    private bool grounded = true;
    private bool movedLastFrame = true;
    private bool moveLocked = false;

    public float fadeTime = 0.5f;

    public float comfortVignetteStrength = 0.5f;

    public float speed = 5.0f;

    private float intendedHeight = 0f;

    void RecenterCamera()
    {
        trackingOffset.localEulerAngles = -head.localEulerAngles.y * Vector3.up;
        trackingOffset.localPosition = -head.localPosition.x * Vector3.right - head.localPosition.z * Vector3.forward + (intendedHeight - head.localPosition.y) * Vector3.up;
    }

    void Start()
    {

        intendedHeight = 1.75f; //trackingOffset.localPosition.y; //TODO: get a better solution that isn't hardcoded

        RecenterCamera();

        body = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        fadeOutEffect = fadeOut.material;

        inputLayer.Initialize(this, RecenterCamera, touch.GetHandInfo);

        SetFade(1f);
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Resolve Input
        inputLayer.ResolveInput(in head, in cameraHolder);

        //Apply camera orientation to camera, NOT body!
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

        movedLastFrame = stop.magnitude != 0f;
        grounded = Physics.Raycast(body.transform.position, -transform.up, coll.bounds.extents.y + 0.1f, ~(1 << 8));

    }

    static private Color blackZero = new Color(0f, 0f, 0f, 0f);
    static private Color blackOne = new Color(0f, 0f, 0f, 1f);

    public IEnumerator Teleport(Vector3 targetPosition, Quaternion targetRotation, bool keepOnHold = false, Action action = null)
    {
        if (moveLocked) yield break;

        moveLocked = true;

        float fade = 0f;
        while (fade < 1f)
        {
            fade = Mathf.Clamp01(fade + Time.unscaledDeltaTime / fadeTime);
            fadeOutEffect.SetColor("_UnlitColor", Color.Lerp(blackZero, blackOne, fade));
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.05f);
        transform.position = targetPosition;
        transform.localRotation = targetRotation;
        yield return new WaitForSecondsRealtime(0.05f);

        while (fade > 0f)
        {
            fade = Mathf.Clamp01(fade - Time.unscaledDeltaTime / fadeTime);
            fadeOutEffect.SetColor("_UnlitColor", Color.Lerp(blackZero, blackOne, fade));
            yield return null;
        }

        if (!keepOnHold)
            moveLocked = false;

        action?.Invoke();
    }

    public IEnumerator FadeIn(Action action = null)
    {
        float fade = 1f;

        while (fade > 0f)
        {
            fade = Mathf.Clamp01(fade - Time.unscaledDeltaTime / fadeTime);
            SetFade(fade);
            yield return null;
        }

        action?.Invoke();
    }

    public void ReleaseMoveLock() => moveLocked = false;

    public bool IsMoveLocked() => moveLocked;

    public void SetNextCheckpoint(Vector3 targetPosition, int currentCheckpoint, int totalCheckpoints)
    {
        nextCheckpoint = targetPosition;
        checkpointGuide.gameObject.SetActive(true);
        checkpointText.text = String.Format("{0}/{1}", currentCheckpoint, totalCheckpoints);
        checkpointLabel.text = "Checkpoint";
    }

    public void DisableCheckpoint()
    {
        checkpointGuide.gameObject.SetActive(false);
        checkpointText.text = "";
        checkpointLabel.text = "";
    }

    public void SetFade(float perc)
    {
        fadeOutEffect.SetColor("_UnlitColor", Color.Lerp(blackZero, blackOne, perc));
    }

    public bool GetTranslationIntent()
    {
        return inputLayer.GetTranslationIntent();
    }
}
