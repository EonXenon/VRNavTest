﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public class InputLayer
{

    private MonoBehaviour mono;

    public Transform lastPickup;

    public enum RotationType
    {
        HeadContinuous,
        MouseContinuous,
        DirectionalContinuous,
        HeadStep,
        DirectionalStep,
        ClickAndChoose
    }

    public enum TranslationType
    {
        Directional,
        DragTowardsGaze,
        DragToPoint,
        Teleport
    }

    public enum InputType
    {
        KeyboardAndMouse,
        Gamepad,
        HandheldControllers,
        TouchSurface
    }

    public RotationType rotationType;
    public TranslationType translationType;
    public InputType inputType;

    [Serializable]
    public struct MouseSettings
    {
        public float sensitivity;
    }

    public MouseSettings mouseSetttings;

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

    public float transitionTime = 0.15f;
    private bool rotating = false;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private bool wasDragging = false;
    private float cumulativeY = 0f;
    private bool hasShot = false;
    private bool hasHit = false;

    public void Initialize(MonoBehaviour mono)
    {
        this.mono = mono;
    }

    public void ResolveInput(in Transform headTransform, in Transform controllerTransform)
    {
        ResolveRotation(in headTransform, in controllerTransform);
        ResolveTranslation(in headTransform, in controllerTransform);

        //Generic Controls

        bool reset = Input.GetButton("Fire2");
        if (reset)
        {
            InputTracking.Recenter();
        }
    }

    private void ResolveRotation(in Transform headTransform, in Transform controllerTransform)
    {
        if (rotationType == RotationType.MouseContinuous && inputType == InputType.KeyboardAndMouse)
        {
            float x = Input.GetAxis("Mouse X");
            targetRotation = Vector3.up * mouseSetttings.sensitivity * x * Time.unscaledDeltaTime;
            return;
        }
        else if (rotationType == RotationType.HeadContinuous)
        {
            float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);
            float resultInput = (headAngle - Mathf.Clamp(headAngle, -headSetttings.deadzoneDegrees, headSetttings.deadzoneDegrees)) * headSetttings.sensitivity;
            targetRotation = Vector3.up * resultInput * Time.unscaledDeltaTime;
            return;
        }
        else if (rotationType == RotationType.DirectionalContinuous)
        {
            float x = Input.GetAxis("TurnAxis");
            targetRotation = Vector3.up * directionalSettings.sensitivity * x * Time.unscaledDeltaTime;
            return;
        }
        else if (rotationType == RotationType.DirectionalStep)
        {
            float x = Mathf.Round(Input.GetAxis("TurnAxis"));

            if (x != 0f && !rotating)
                mono.StartCoroutine(DoRotateStep(x * directionalSettings.stepSize, controllerTransform));

            return;
        }
        else if (rotationType == RotationType.HeadStep)
        {
            float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);
            float resultInput = (headAngle - Mathf.Clamp(headAngle, -headSetttings.deadzoneDegrees, headSetttings.deadzoneDegrees));

            if (resultInput != 0f && !rotating)
                mono.StartCoroutine(DoRotateStep(Mathf.Sign(resultInput) * headSetttings.stepSize, controllerTransform));
            return;
        }

        Debug.LogError("The requested configuration scheme does not exist!");
    }
    
    private IEnumerator DoRotateStep(float rotation, Transform controllerTransform)
    {
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
        if (translationType == TranslationType.Directional)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            float u = Input.GetAxis("Upward");

            targetTranslation = (controllerTransform.right * h + controllerTransform.forward * v + controllerTransform.up * u);
            return;
        }
        else if (translationType == TranslationType.DragTowardsGaze)
        {
            float y = Input.GetAxis("Mouse Y");
            bool dragging = Input.GetButton("Fire1");

            if (dragging)
                targetTranslation += headTransform.forward * -y * mouseSetttings.sensitivity * Time.unscaledDeltaTime;

            return;
        }
        else if (translationType == TranslationType.DragToPoint)
        {
            float y = Input.GetAxis("Mouse Y");
            bool dragging = Input.GetButton("Fire1");

            if (dragging)
            {
                RaycastHit hit;

                if(!hasShot)
                {
                    hasHit = Physics.Raycast(headTransform.position, headTransform.forward, out hit, 500f);
                    if (hasHit)
                    {
                        startPos = controllerTransform.position;
                        targetPos = hit.point - headTransform.localPosition; //Correct for head height
                        lastPickup.position = hit.point;
                    }

                    hasShot = true;
                }

                if(!wasDragging)
                {
                    wasDragging = true;
                }
                else if(hasHit)
                {
                    targetTranslation = Vector3.Lerp(startPos, targetPos, cumulativeY) - controllerTransform.position;
                }
                
                cumulativeY = Mathf.Clamp01(cumulativeY + Time.unscaledDeltaTime);
            }
            else
            {
                wasDragging = false;
                cumulativeY = 0f;
                hasShot = false;
                lastPickup.position = Vector3.one * 99999f;
            }

            return;


        }
        else if (translationType == TranslationType.Teleport)
        {
            //bool teleport = false;

            //if (teleport && !teleporting)
            //StartCoroutine(Teleport(Vector3.up, Quaternion.identity));

            Debug.LogError("Teleport is not yet functional!");

            return;
        }

        Debug.LogError("The requested configuration scheme does not exist!");
    }

    public Vector3 GetCumulativeTranslationInput()
    {
        Vector3 normalizedResult = targetTranslation;
        if (translationType == TranslationType.Directional)
            normalizedResult /= Mathf.Max(1f, targetTranslation.magnitude);

        targetTranslation = Vector3.zero;
        return normalizedResult;
    }

    public Vector3 GetCumulativeRotationInput()
    {
        Vector3 result = targetRotation;
        targetRotation = Vector3.zero;
        return result;
    }
}
