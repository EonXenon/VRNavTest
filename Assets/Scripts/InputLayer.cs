using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputLayer
{
    public enum RotationType
    {
        HeadContinuous,
        MouseContinuous,
        DirectionalContinuous,
        HeadStep,
        MouseStep,
        DirectionalStep,
        ClickAndChoose
    }

    public enum TranslationType
    {
        Directional,
        DragTowardsGaze,
        DragToPoint,
        Teleport,
        LinearMotion
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
    }

    public HeadSettings headSetttings;

    public void ResolveRotation(in Transform headTransform, ref Transform controllerTransform)
    {
        if (rotationType == RotationType.MouseContinuous && inputType == InputType.KeyboardAndMouse)
        {
            float x = Input.GetAxis("Mouse X");
            controllerTransform.localEulerAngles = controllerTransform.transform.localEulerAngles + Vector3.up * mouseSetttings.sensitivity * x * Time.unscaledDeltaTime;
            return;
        }
        else if (rotationType == RotationType.HeadContinuous)
        {
            float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);
            float resultInput = (headAngle - Mathf.Clamp(headAngle, -headSetttings.deadzoneDegrees, headSetttings.deadzoneDegrees)) * headSetttings.sensitivity;
            controllerTransform.localEulerAngles = controllerTransform.transform.localEulerAngles + Vector3.up * resultInput * Time.unscaledDeltaTime;
            return;
        }

        Debug.LogError("The requested configuration scheme does not exist!");
    }
}
