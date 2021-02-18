using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform cameraHolder = null;
    private Rigidbody body;
    private CapsuleCollider coll;
    [SerializeField]
    private MeshRenderer flightGrid;
    [SerializeField]
    private MeshRenderer fadeOut;
    private Material flightGridEffect;
    private Material fadeOutEffect;

    private bool grounded = true;
    private bool teleporting = false;
    private float gridFade = 0f;
    private float fadeTime = 0.5f;

    public float sensitivity = 25.0f;
    public float speed = 5.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        InputTracking.Recenter();
        
        body = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        flightGridEffect = flightGrid.material;
        fadeOutEffect = fadeOut.material;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        bool teleport = Input.GetButton("Fire1");
        bool reset = Input.GetButton("Fire2");

        if (teleport && !teleporting)
            StartCoroutine(Teleport(Vector3.up, Quaternion.identity));

        if (grounded)
        {
            gridFade = Mathf.Clamp01(gridFade - Time.unscaledDeltaTime / fadeTime);
        }
        else
        {
            gridFade = Mathf.Clamp01(gridFade + Time.unscaledDeltaTime / fadeTime);
        }

        flightGridEffect.SetFloat("_fadeEffect", gridFade);

        //Rotate the camera independently of body for maximum smoothness, just remember to sync the body!
        if (cameraHolder != null)
            cameraHolder.localEulerAngles = cameraHolder.transform.localEulerAngles + Vector3.up * sensitivity * x * Time.deltaTime;

        if (reset)
        {
            InputTracking.Recenter();
        }
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float u = Input.GetAxis("Upward");

        //Apply camera orientation to player orientation, and re-sync camera with object
        //This will mean that a player, like any object in the game, will update at the same fixed rate, so the behavior will be very predictable, while retaining a smooth visual behavior
        //Don't forget to set the Rigidbody to Interpolate!
        if (cameraHolder != null)
        {
            transform.Rotate(transform.up, cameraHolder.localEulerAngles.y);
            cameraHolder.localEulerAngles = Vector3.zero;
        }

        Vector3 slow = -body.velocity;
        Vector3 move = (transform.right * h + transform.forward * v + transform.up * u);
        move *= speed / Mathf.Max(1f, move.magnitude);

        if(!teleporting)
            body.AddForce(move + slow, ForceMode.VelocityChange);
        else body.AddForce(slow, ForceMode.VelocityChange);

        grounded = Physics.Raycast(body.transform.position, -transform.up, coll.bounds.extents.y + 0.1f, ~(1 << 8));

    }

    static private Color blackZero = new Color(0f, 0f, 0f, 0f);
    static private Color blackOne = new Color(0f, 0f, 0f, 1f);

    IEnumerator Teleport(Vector3 targetPosition, Quaternion targetRotation)
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
