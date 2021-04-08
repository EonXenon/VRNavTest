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
    private MeshRenderer flightGrid;
    [SerializeField]
    private Volume postVolume;
    private Vignette vignetteEffect;
    [SerializeField]
    private MeshRenderer fadeOut;
    private Material flightGridEffect;
    private Material fadeOutEffect;
    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform trackingOffset;

    [SerializeField]
    private TouchController touch;

    [SerializeField]
    private InputLayer inputLayer;

    private bool grounded = true;
    private bool movedLastFrame = true;
    private bool teleporting = false;
    private float gridOpacity = 0f;

    public float fadeTime = 0.5f;

    public float comfortVignetteStrength = 0.5f;

    public float speed = 5.0f;

    private float intendedHeight = 0f;

    void RecenterCamera()
    {
        //TODO: Replace this with a proper system
        //XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        //InputTracking.Recenter();
        trackingOffset.localEulerAngles = -head.localEulerAngles.y * Vector3.up;
        trackingOffset.localPosition = -head.localPosition.x * Vector3.right - head.localPosition.z * Vector3.forward + (intendedHeight - head.localPosition.y) * Vector3.up;
    }

    void Start()
    {

        intendedHeight = 1.75f; //trackingOffset.localPosition.y; //TODO: get a better solution that isn't hardcoded
        //Cursor.lockState = CursorLockMode.Locked;

        RecenterCamera();

        body = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        flightGridEffect = flightGrid.material;
        fadeOutEffect = fadeOut.material;
        postVolume.profile.TryGet<Vignette>(out vignetteEffect);

        inputLayer.Initialize(this, RecenterCamera, touch.GetHandInfo);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Resolve Input
        inputLayer.ResolveInput(in head, in cameraHolder);

        //Apply camera orientation to camera, NOT body!
        cameraHolder.localEulerAngles += inputLayer.GetCumulativeRotationInput();

        //Grid effect code
        gridOpacity = Mathf.Clamp01(gridOpacity - (grounded||!movedLastFrame ? 1 : -1) * Time.unscaledDeltaTime / fadeTime);
        flightGridEffect.SetFloat("_fadeEffect", gridOpacity);
        vignetteEffect.intensity.value = gridOpacity * comfortVignetteStrength;
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

        body.AddForce(move + stop, ForceMode.VelocityChange);

        movedLastFrame = stop.magnitude != 0f;
        grounded = Physics.Raycast(body.transform.position, -transform.up, coll.bounds.extents.y + 0.1f, ~(1 << 8));

    }

    static private Color blackZero = new Color(0f, 0f, 0f, 0f);
    static private Color blackOne = new Color(0f, 0f, 0f, 1f);

    public IEnumerator Teleport(Vector3 targetPosition, Quaternion targetRotation)
    {
        teleporting = true;

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

        teleporting = false;
    }
}
