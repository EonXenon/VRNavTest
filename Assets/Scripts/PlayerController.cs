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
    private Material flightGridEffect;

    private bool grounded = true;
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        bool reset = Input.GetButton("Fire1");

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
        if (cameraHolder != null)
        {
            transform.Rotate(transform.up, cameraHolder.localEulerAngles.y);
            cameraHolder.localEulerAngles = Vector3.zero;
        }

        Vector3 slow = -body.velocity;
        Vector3 move = (transform.right * h + transform.forward * v + transform.up * u);
        move *= speed / Mathf.Max(1f, move.magnitude);

        body.AddForce(move + slow, ForceMode.VelocityChange);

        grounded = Physics.Raycast(body.transform.position, -transform.up, coll.bounds.extents.y + 0.1f, ~(1 << 8));

    }
}
