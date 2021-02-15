using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform cameraHolder;

    public float sensitivity = 25.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //Rotate the camera independently of body for maximum smoothness, just remember to sync the body!
        cameraHolder.localEulerAngles = cameraHolder.transform.localEulerAngles + Vector3.up * sensitivity * x * Time.deltaTime;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //Apply camera orientation to player orientation, and re-sync camera with object
        //This will mean that a player, like any object in the game, will update at the same fixed rate, so the behavior will be very predictable, while retaining a smooth visual behavior
        transform.Rotate(transform.up, cameraHolder.localEulerAngles.y);
        cameraHolder.localEulerAngles = Vector3.zero;
    }
}
