﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using static InputMaster;

[Serializable]
public class InputLayer : IPlayerActions
{
    private MonoBehaviour mono;

    private Action recenterAction;
    private Func<SimpleHandTouch> handInfoAction;

    private InputMaster config;

    public Transform lastPickup;

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

    [Serializable]
    public struct MouseSettings
    {
        public float sensitivity;
    }

    public MouseSettings mouseSetttings;

    [Serializable]
    public struct TouchSettings
    {
        public float rotationSensitivity;
        public float dragSensitivity;
    }

    public TouchSettings touchSettings;

    [Serializable]
    public struct HeadSettings
    {
        public float sensitivity;
        public float deadzoneDegrees;
        public float stepSize;
    }

    public HeadSettings headSetttings;

    [Serializable]
    public struct DirectionalSettings
    {
        public float sensitivity;
        public float stepSize;
    }

    public DirectionalSettings directionalSettings;

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

    //Generalized input
    [Serializable]
    public struct AbstractedInput
    {
        public bool translationFunction;
        public bool rotationFunction;
        public Vector3 directionalInput;
        public float turnAxis;
        public float turnDirect;
        public float dragDirect;
        public Vector2 analogLook;
        public Quaternion handDirection;
    }

    private AbstractedInput abstractedInput;

    public void Initialize(MonoBehaviour mono, Action resetAction, Func<SimpleHandTouch> handAction)
    {
        this.mono = mono;
        this.recenterAction = resetAction;
        this.handInfoAction = handAction;

        config = new InputMaster();
        config.Enable();

        config.Player.Recenter.started += OnRecenter;

        config.Player.RotationFunction.started += OnRotationFunction;
        config.Player.RotationFunction.canceled += OnRotationFunction;

        config.Player.TranslationFunction.started += OnTranslationFunction;
        config.Player.TranslationFunction.canceled += OnTranslationFunction;

        config.Player.TranslationXZAxis.started += OnTranslationXZAxis;
        config.Player.TranslationXZAxis.performed += OnTranslationXZAxis;
        config.Player.TranslationXZAxis.canceled += OnTranslationXZAxis;

        config.Player.TranslationYAxis.started += OnTranslationYAxis;
        config.Player.TranslationYAxis.performed += OnTranslationYAxis;
        config.Player.TranslationYAxis.canceled += OnTranslationYAxis;

        config.Player.TurnAxis.started += OnTurnAxis;
        config.Player.TurnAxis.performed += OnTurnAxis;
        config.Player.TurnAxis.canceled += OnTurnAxis;

        config.Player.AnalogLook.started += OnAnalogLook;
        config.Player.AnalogLook.performed += OnAnalogLook;
        config.Player.AnalogLook.canceled += OnAnalogLook;

        config.Player.TurnDirect.started += OnTurnDirect;
        config.Player.TurnDirect.performed += OnTurnDirect;
        config.Player.TurnDirect.canceled += OnTurnDirect;

        config.Player.DragDirect.started += OnDragDirect;
        config.Player.DragDirect.performed += OnDragDirect;
        config.Player.DragDirect.canceled += OnDragDirect;


        config.Player.HandOrientation.started += OnHandOrientation;
        config.Player.HandOrientation.performed += OnHandOrientation;
        config.Player.HandOrientation.canceled += OnHandOrientation;
    }

    public void ResolveInput(in Transform headTransform, in Transform controllerTransform)
    {
        ResolveRotation(in headTransform, in controllerTransform);
        ResolveTranslation(in headTransform, in controllerTransform);
    }

    private void ResolveRotation(in Transform headTransform, in Transform controllerTransform)
    {
        switch (rotationType)
        {
            case RotationType.Drag:
                {
                    float x = handInfoAction().leftHandDelta.x + handInfoAction().rightHandDelta.x;
                    if (handInfoAction().leftHandPress && handInfoAction().rightHandPress) x /= 2f;
                    targetRotation = Vector3.up * touchSettings.rotationSensitivity * -x * Time.unscaledDeltaTime;

                    return;
                }

            case RotationType.DirectionalContinuous:
                {
                    float x = abstractedInput.turnAxis;
                    targetRotation = Vector3.up * directionalSettings.sensitivity * x * Time.unscaledDeltaTime;
                    return;
                }

            case RotationType.ClickAndChoose:
                {
                    if (abstractedInput.rotationFunction)
                    {
                        targetOrientation = abstractedInput.analogLook;
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
                    if (abstractedInput.rotationFunction && !wasHolding)
                    {
                        float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);

                        if (headAngle != 0f)
                            mono.StartCoroutine(DoRotateStep(headAngle, controllerTransform));

                        wasHolding = true;
                    }
                    else if (wasHolding)
                    {
                        wasHolding = false;
                    }

                    return;
                }
            case RotationType.DevMode:
                {
                    float x = abstractedInput.turnDirect;
                    targetRotation = Vector3.up * mouseSetttings.sensitivity * x * Time.unscaledDeltaTime;
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
                    Quaternion finalTransformation = controllerTransform.rotation * abstractedInput.handDirection;
                    targetTranslation = (finalTransformation  * Vector3.right * abstractedInput.directionalInput.x
                            + finalTransformation * Vector3.forward * abstractedInput.directionalInput.z
                            + finalTransformation * Vector3.up * abstractedInput.directionalInput.y);

                    return;
                }

            case TranslationType.Paddling:
                {
                    float y = handInfoAction().leftHandDelta.y + handInfoAction().rightHandDelta.y;
                    if (handInfoAction().leftHandPress && handInfoAction().rightHandPress) y /= 2f;
                    targetTranslation += headTransform.forward * -y * touchSettings.dragSensitivity * Time.unscaledDeltaTime;

                    return;
                }

            case TranslationType.DragNGo:
                {
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

                    return;
                }

            case TranslationType.DevMode:
                {
                    targetTranslation
                        =(controllerTransform.right * abstractedInput.directionalInput.x
                        + controllerTransform.forward * abstractedInput.directionalInput.z
                        + controllerTransform.up * abstractedInput.directionalInput.y);
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
    public bool GetRotationIntent() => targetTranslation.magnitude > 0f;

    public Vector3 GetCumulativeRotationInput()
    {
        Vector3 result = targetRotation;
        targetRotation = Vector3.zero;
        return result;
    }

    public float GetIntendedSpeedMultiplier(float baseSpeed, float topSpeed)
    {
        if (translationType == TranslationType.Directional || translationType == TranslationType.DevMode)
        {
            if (abstractedInput.translationFunction) return topSpeed;
            else return baseSpeed;
        }
        else return 1f;
    }

    public void OnTranslationXZAxis(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();
        abstractedInput.directionalInput.x = temp.x;
        abstractedInput.directionalInput.z = temp.y;
    }

    public void OnRecenter(InputAction.CallbackContext context) => recenterAction();
    public void OnTranslationFunction(InputAction.CallbackContext context) => abstractedInput.translationFunction = context.ReadValue<float>() > 0.5f;
    public void OnRotationFunction(InputAction.CallbackContext context) => abstractedInput.rotationFunction = context.ReadValue<float>() > 0.5f;
    public void OnTranslationYAxis(InputAction.CallbackContext context) => abstractedInput.directionalInput.y = context.ReadValue<float>();
    public void OnTurnAxis(InputAction.CallbackContext context) => abstractedInput.turnAxis = context.ReadValue<float>();
    public void OnTurnDirect(InputAction.CallbackContext context) => abstractedInput.turnDirect = context.ReadValue<Vector2>().x;
    public void OnDragDirect(InputAction.CallbackContext context) => abstractedInput.dragDirect = context.ReadValue<float>();
    public void OnAnalogLook(InputAction.CallbackContext context) => abstractedInput.analogLook = context.ReadValue<Vector2>();
    public void OnHandOrientation(InputAction.CallbackContext context) => abstractedInput.handDirection = context.ReadValue<Quaternion>();
}
