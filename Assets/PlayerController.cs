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

    public float sensitivity = 25.0f;
    public float speed = 5.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //Rotate the camera independently of body for maximum smoothness, just remember to sync the body!
        if (cameraHolder != null)
            cameraHolder.localEulerAngles = cameraHolder.transform.localEulerAngles + Vector3.up * sensitivity * x * Time.deltaTime;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //Apply camera orientation to player orientation, and re-sync camera with object
        //This will mean that a player, like any object in the game, will update at the same fixed rate, so the behavior will be very predictable, while retaining a smooth visual behavior
        if (cameraHolder != null)
        {
            transform.Rotate(transform.up, cameraHolder.localEulerAngles.y);
            cameraHolder.localEulerAngles = Vector3.zero;
        }

        Vector3 slow = Vector3.Scale(Vector3.one - Vector3.up, -body.velocity);
        Vector3 move = (transform.right * h + transform.forward * v);
        move *= speed / Mathf.Max(1f, move.magnitude);

        body.AddForce(move + slow, ForceMode.VelocityChange);

    }
}
