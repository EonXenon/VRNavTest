using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class InputLayer
{
    private MonoBehaviour mono;

    private Action recenterAction;

    private InputMaster config;

    public Transform hitPoint;

    public enum RotationType
    {
        Drag,
        DirectionalContinuous,
        ClickAndChoose,
        HeadConverge,
        DevMode
    }

    public enum TranslationType
    {
        Directional,
        Paddling,
        DragNGo,
        DevMode
    }

    public RotationType rotationType;
    public TranslationType translationType;
    RotationType? oldRotationType = null;
    TranslationType? oldTranslationType = null;

    [Serializable]
    public struct DevModeSettings
    {
        public float sensitivity;
    }

    public DevModeSettings devModeSettings;

    private Vector3 targetTranslation = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;
    private Vector2 targetOrientation = Vector2.up;

    public float transitionTime = 0.15f;
    private bool rotating = false;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector3 savedNormal = Vector3.forward;
    private float defaultDistance = 500f;
    private bool wasDragging = false;
    private bool wasHolding = false;
    private float cumulativeY = 0f;
    private bool hasShot = false;
    private bool hasHit = false;

    struct InputData
    {
        public float drag_delta;
        public float dCont_rotate;
        public Vector2 cnC_thumb;
        public bool headConv_conv;
        public float devMode_delta;
        public bool direct_func;
        public Quaternion direct_handOri;
        public Vector3 direct_handPos;
        public bool devMode_boost;
        public Vector3 devMode_trans;
    }

    InputData data;

    public float dragRotationSensitivity;
    public float directionalContinuousRotationSpeed;
    public float paddlingTranslationSpeed;


    //Touch related
    struct HandData
    {
        public Vector2 rightHandPosition;
        public Vector2 rightHandDelta;
        public bool rightHandTouch;

        public Vector2 leftHandPosition;
        public Vector2 leftHandDelta;
        public bool leftHandTouch;

        public Dictionary<int, SimpleTouch> rightHandTouches;
        public Dictionary<int, SimpleTouch> leftHandTouches;
    }

    HandData handData;

    public RectTransform handInputUI;
    public Transform[] fingers;



    //Generalized input

    public void Initialize(MonoBehaviour mono, Action resetAction)
    {
        this.mono = mono;
        this.recenterAction = resetAction;

        handData.rightHandTouches = new Dictionary<int, SimpleTouch>();
        handData.rightHandDelta = Vector2.zero;
        handData.rightHandTouch = false;
        
        handData.leftHandTouches = new Dictionary<int, SimpleTouch>();
        handData.leftHandDelta = Vector2.zero;
        handData.leftHandTouch = false;

        config = new InputMaster();
        config.Enable();

        //Generic
        config.Player.Recenter.started += OnRecenter;


        //R_DevMode
        config.R_DevMode.Rotate.started += R_DevMode_Delta;
        config.R_DevMode.Rotate.performed += R_DevMode_Delta;
        config.R_DevMode.Rotate.canceled += R_DevMode_Delta;
        config.R_DevMode.Disable();

        //R_DirectionalContinuous
        config.R_DirectionalContinuous.Rotate.started += R_DirectionalContinuous_Rotate;
        config.R_DirectionalContinuous.Rotate.performed += R_DirectionalContinuous_Rotate;
        config.R_DirectionalContinuous.Rotate.canceled += R_DirectionalContinuous_Rotate;
        config.R_DirectionalContinuous.Disable();

        //R_ClickAndChoose
        config.R_ClickAndChoose.Direction.started += R_ClickAndChoose_Direction;
        config.R_ClickAndChoose.Direction.performed += R_ClickAndChoose_Direction;
        config.R_ClickAndChoose.Direction.canceled += R_ClickAndChoose_Direction;
        config.R_ClickAndChoose.Disable();

        //R_HeadConverge
        config.R_HeadConverge.RotationFunction.started += R_HeadConverge_Function;
        config.R_HeadConverge.Disable();

        //T_DevMode
        config.T_DevMode.SpeedBoost.started += T_DevMode_SpeedBoost;
        config.T_DevMode.SpeedBoost.performed += T_DevMode_SpeedBoost;
        config.T_DevMode.SpeedBoost.canceled += T_DevMode_SpeedBoost;
        config.T_DevMode.TranslationXAxis.started += T_DevMode_TX;
        config.T_DevMode.TranslationXAxis.performed += T_DevMode_TX;
        config.T_DevMode.TranslationXAxis.canceled += T_DevMode_TX;
        config.T_DevMode.TranslationYAxis.started += T_DevMode_TY;
        config.T_DevMode.TranslationYAxis.performed += T_DevMode_TY;
        config.T_DevMode.TranslationYAxis.canceled += T_DevMode_TY;
        config.T_DevMode.TranslationZAxis.started += T_DevMode_TZ;
        config.T_DevMode.TranslationZAxis.performed += T_DevMode_TZ;
        config.T_DevMode.TranslationZAxis.canceled += T_DevMode_TZ;
        config.T_DevMode.Disable();

        //T_Directional
        config.T_Directional.Move.started += T_Directional_Move;
        config.T_Directional.Move.performed += T_Directional_Move;
        config.T_Directional.Move.canceled += T_Directional_Move;
        config.T_Directional.HandOrientation.started += T_Directional_Ori;
        config.T_Directional.HandOrientation.performed += T_Directional_Ori;
        config.T_Directional.HandOrientation.canceled += T_Directional_Ori;
        config.T_Directional.HandPosition.started += T_Directional_Pos;
        config.T_Directional.HandPosition.performed += T_Directional_Pos;
        config.T_Directional.HandPosition.canceled += T_Directional_Pos;
        config.T_Directional.Disable();

    }

    public void ResolveInput(in Transform headTransform, in Transform controllerTransform)
    {
        CheckforInputSwap();
        ProcessHands();

        ResolveRotation(in headTransform, in controllerTransform);
        ResolveTranslation(in headTransform, in controllerTransform);
    }

    private void CheckforInputSwap()
    {
        if (rotationType != oldRotationType)
        {
            switch (oldRotationType)
            {
                case RotationType.DirectionalContinuous:
                    {
                        config.R_DirectionalContinuous.Disable();
                        break;
                    }

                case RotationType.ClickAndChoose:
                    {
                        config.R_ClickAndChoose.Disable();
                        break;
                    }

                case RotationType.HeadConverge:
                    {
                        config.R_HeadConverge.Disable();
                        break;
                    }

                case RotationType.DevMode:
                    {
                        config.R_DevMode.Disable();
                        break;
                    }
            }

            oldRotationType = rotationType;

            switch (rotationType)
            {
                case RotationType.DirectionalContinuous:
                    {
                        config.R_DirectionalContinuous.Enable();
                        break;
                    }

                case RotationType.ClickAndChoose:
                    {
                        config.R_ClickAndChoose.Enable();
                        break;
                    }

                case RotationType.HeadConverge:
                    {
                        config.R_HeadConverge.Enable();
                        break;
                    }

                case RotationType.DevMode:
                    {
                        config.R_DevMode.Enable();
                        break;
                    }
            }
        }

        if (translationType != oldTranslationType)
        {
            switch (oldTranslationType)
            {
                case TranslationType.Directional:
                    {
                        config.T_Directional.Disable();
                        break;
                    }

                case TranslationType.DevMode:
                    {
                        config.T_DevMode.Disable();
                        break;
                    }

            }

            oldTranslationType = translationType;

            switch (translationType)
            {
                case TranslationType.Directional:
                    {
                        config.T_Directional.Enable();
                        break;
                    }

                case TranslationType.DevMode:
                    {
                        config.T_DevMode.Enable();
                        break;
                    }

            }
        }
    }

    void ProcessHands()
    {

        Dictionary<int, SimpleTouch> newRightHandTouches = new Dictionary<int, SimpleTouch>();
        Dictionary<int, SimpleTouch> newLeftHandTouches = new Dictionary<int, SimpleTouch>();

        if (Touchscreen.current == null)
        {
            Debug.LogError("Touchscreen hardware appears to be missing! Restart application and try again!");
        }

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            SimpleTouch t = new SimpleTouch(Touchscreen.current.touches[i]);

            if (!t.pressed)
            {
                fingers[i].gameObject.SetActive(false);
                continue;
            }

            if (handData.rightHandTouches.ContainsKey(t.id))
            {
                newRightHandTouches[t.id] = t;
            }
            else if (handData.leftHandTouches.ContainsKey(t.id))
            {
                newLeftHandTouches[t.id] = t;
            }
            else if (t.position.x > 0.5f)
            {
                newRightHandTouches[t.id] = t;
            }
            else
            {
                newLeftHandTouches[t.id] = t;
            }

            fingers[i].localPosition = Vector3.up * (t.position.y - 0.5f) * handInputUI.sizeDelta.y + Vector3.right * (t.position.x - 0.5f) * handInputUI.sizeDelta.x;
            fingers[i].gameObject.SetActive(true);
        }

        handData.rightHandTouches = new Dictionary<int, SimpleTouch>(newRightHandTouches);
        handData.leftHandTouches = new Dictionary<int, SimpleTouch>(newLeftHandTouches);

        List<SimpleTouch> rightHandTouchesList = handData.rightHandTouches.Values.ToList();
        List<SimpleTouch> leftHandTouchesList = handData.leftHandTouches.Values.ToList();

        // Right Hand
        handData.rightHandTouch = rightHandTouchesList.Count > 0;

        if (handData.rightHandTouch)
        {
            // Calc new center
            handData.rightHandPosition = SimpleTouch.CalculateHandPosition(rightHandTouchesList);
            handData.rightHandDelta = SimpleTouch.CalculateHandDelta(rightHandTouchesList);
        }

        // Left Hand
        handData.leftHandTouch = leftHandTouchesList.Count > 0;

        if (handData.leftHandTouch)
        {
            // Calc new center
            handData.leftHandPosition = SimpleTouch.CalculateHandPosition(leftHandTouchesList);
            handData.leftHandDelta = SimpleTouch.CalculateHandDelta(leftHandTouchesList);
        }

    }

    private void ResolveRotation(in Transform headTransform, in Transform controllerTransform)
    {
        switch (rotationType)
        {
            case RotationType.Drag:
                {
                    float x = handData.leftHandDelta.x + handData.rightHandDelta.x;
                    if (handData.leftHandTouch && handData.rightHandTouch) x /= 2f;
                    targetRotation = Vector3.up * dragRotationSensitivity * -x * Time.unscaledDeltaTime;

                    return;
                }

            case RotationType.DirectionalContinuous:
                {
                    float x = data.dCont_rotate;
                    targetRotation = Vector3.up * directionalContinuousRotationSpeed * x * Time.unscaledDeltaTime;
                    return;
                }

            case RotationType.ClickAndChoose:
                {
                    if (data.cnC_thumb.magnitude > 0.75f)
                    {
                        targetOrientation = data.cnC_thumb;
                        wasHolding = true;
                    }
                    else if (wasHolding)
                    {
                        wasHolding = false;
                        float resultInput = Vector2.SignedAngle(targetOrientation.normalized, Vector2.up);
                        if (resultInput != 0f)
                            mono.StartCoroutine(DoRotateStep(resultInput, controllerTransform));

                        targetOrientation = Vector2.up;
                    }

                    return;
                }

            case RotationType.HeadConverge:
                {
                    if (data.headConv_conv)
                    {
                        float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);

                        if (headAngle != 0f)
                            mono.StartCoroutine(DoRotateStep(headAngle, controllerTransform));

                        data.headConv_conv = false;
                    }

                    return;
                }
            case RotationType.DevMode:
                {
                    float x = data.devMode_delta;
                    targetRotation = Vector3.up * devModeSettings.sensitivity * x * Time.unscaledDeltaTime;
                    return;
                }
        }

        Debug.LogError("The requested configuration scheme does not exist!");
    }
    
    private IEnumerator DoRotateStep(float rotation, Transform controllerTransform)
    {
        if (rotating) yield break;

        rotating = true;

        //We change the parent rotation during this animation, so we need to change the non-local rotation
        Vector3 originalAngle = controllerTransform.eulerAngles;

        Vector3 finalAngle = originalAngle + Vector3.up * rotation;

        float rotated = 0f;
        while (rotated < 1f)
        {
            rotated = Mathf.Clamp01(rotated + Time.unscaledDeltaTime / transitionTime);
            targetRotation += Vector3.Lerp(originalAngle, finalAngle, rotated) - controllerTransform.eulerAngles;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.1f);

        rotating = false;
    }

    private void ResolveTranslation(in Transform headTransform, in Transform controllerTransform)
    {
        switch (translationType)
        {
            case TranslationType.Directional:
                {
                    Quaternion finalTransformation = controllerTransform.rotation * data.direct_handOri;
                    if(data.direct_func)
                        targetTranslation = finalTransformation * Vector3.forward;

                    return;
                }

            case TranslationType.Paddling:
                {
                    float y = handData.leftHandDelta.y + handData.rightHandDelta.y;
                    if (handData.leftHandTouch && handData.rightHandTouch) y /= 2f;
                    targetTranslation += headTransform.forward * -y * paddlingTranslationSpeed * Time.unscaledDeltaTime;

                    return;
                }

            case TranslationType.DragNGo:
                {
                    //This needs to be done from the ground up
                    /*
                    float y = abstractedInput.dragDirect;

                    bool hasHitOnce = Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit hit, defaultDistance);

                    if (abstractedInput.translationFunction)
                    {
                        if (!hasShot)
                        {
                            if (hasHitOnce)
                            {
                                hasHit = true;

                                startPos = controllerTransform.position;
                                targetPos = hit.point - headTransform.localPosition; //Correct for head height
                                lastPickup.position = hit.point;
                                savedNormal = -hit.normal;
                                lastPickup.rotation = Quaternion.LookRotation(savedNormal);
                            }
                            else
                            {
                                startPos = controllerTransform.position;
                                targetPos = headTransform.position + headTransform.forward * defaultDistance - headTransform.localPosition; //Correct for head height
                                lastPickup.position = headTransform.position + headTransform.forward * defaultDistance;
                                savedNormal = headTransform.forward;
                                lastPickup.rotation = Quaternion.LookRotation(savedNormal);
                            }

                            hasShot = true;
                        }

                        if (!wasDragging)
                        {
                            wasDragging = true;
                        }
                        else if (hasHit)
                        {
                            targetTranslation = Vector3.Lerp(startPos, targetPos, cumulativeY) - controllerTransform.position;
                            lastPickup.position = targetPos + headTransform.localPosition;
                            lastPickup.rotation = Quaternion.LookRotation(savedNormal);
                        }

                        cumulativeY = Mathf.Clamp01(cumulativeY + -y * mouseSetttings.sensitivity * Time.unscaledDeltaTime);
                    }
                    else
                    {
                        wasDragging = false;
                        cumulativeY = 0f;
                        hasShot = false;
                        hasHit = false;

                        if (hasHitOnce)
                        {
                            lastPickup.position = hit.point;
                            lastPickup.rotation = Quaternion.LookRotation(-hit.normal);
                        }
                        else
                        {
                            lastPickup.position = headTransform.position + headTransform.forward * defaultDistance;
                            lastPickup.rotation = Quaternion.LookRotation(headTransform.forward);
                        }
                    }


                    lastPickup.localScale = Vector3.one * (0.1f + Vector3.Distance(headTransform.position, lastPickup.position) / 15f);
                    */
                    return;
                }

            case TranslationType.DevMode:
                {
                    targetTranslation
                        =(controllerTransform.right * data.devMode_trans.x
                        + controllerTransform.forward * data.devMode_trans.z
                        + controllerTransform.up * data.devMode_trans.y);
                }

                return;
        }

        Debug.LogError("The requested configuration scheme does not exist!");
    }

    public Vector3 GetCumulativeTranslationInput()
    {
        Vector3 normalizedResult = targetTranslation;
        if (translationType == TranslationType.Directional || translationType == TranslationType.DevMode)
            normalizedResult /= Mathf.Max(1f, targetTranslation.magnitude);

        targetTranslation = Vector3.zero;
        return normalizedResult;
    }

    public bool GetTranslationIntent() => targetTranslation.magnitude > 0f;
    public bool GetRotationIntent() => targetRotation.magnitude > 0f;

    public Vector3 GetCumulativeRotationInput()
    {
        Vector3 result = targetRotation;
        targetRotation = Vector3.zero;
        return result;
    }

    public float GetIntendedSpeedMultiplier(float baseSpeed, float topSpeed)
    {
        if (translationType == TranslationType.Directional)
        {
            return topSpeed;
        }
        else if (translationType == TranslationType.DevMode)
        {
            if (data.devMode_boost) return topSpeed;
            else return baseSpeed;
        }
        else return 1f;
    }

    public void OnRecenter(InputAction.CallbackContext context) => recenterAction();
    void R_DevMode_Delta(InputAction.CallbackContext context) => data.devMode_delta = context.ReadValue<float>();
    void R_DirectionalContinuous_Rotate(InputAction.CallbackContext context) => data.dCont_rotate = context.ReadValue<float>();
    void R_ClickAndChoose_Direction(InputAction.CallbackContext context) => data.cnC_thumb = context.ReadValue<Vector2>();
    void R_HeadConverge_Function(InputAction.CallbackContext context) => data.headConv_conv = context.ReadValue<float>()>0f;
    void T_DevMode_SpeedBoost(InputAction.CallbackContext context) => data.devMode_boost = context.ReadValue<float>()>0f;
    void T_DevMode_TX(InputAction.CallbackContext context) => data.devMode_trans.x = context.ReadValue<float>();
    void T_DevMode_TY(InputAction.CallbackContext context) => data.devMode_trans.y = context.ReadValue<float>();
    void T_DevMode_TZ(InputAction.CallbackContext context) => data.devMode_trans.z = context.ReadValue<float>();
    void T_Directional_Move(InputAction.CallbackContext context) => data.direct_func = context.ReadValue<float>() > 0f;
    void T_Directional_Ori(InputAction.CallbackContext context) => data.direct_handOri = context.ReadValue<Quaternion>();
    void T_Directional_Pos(InputAction.CallbackContext context) => data.direct_handPos = context.ReadValue<Vector3>();

}
