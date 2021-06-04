using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class InputLayer
{
    private MonoBehaviour mono;
    private Action recenterAction;
    private InputMaster config;

    public enum RotationType
    {
        DirectionalContinuous,
        ClickAndChoose,
        HeadConverge,
        Drag,
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
    private Vector3? targetPosition = null;
    private Vector3 targetRotation = Vector3.zero;
    private Vector2 targetOrientation = Vector2.up;

    public float transitionTime = 0.15f;
    private bool rotating = false;

    private bool wasHolding = false;


    //Drag'n'Go
    Queue<float> interpolatedDragNGo = new Queue<float>();
    bool isTargetSet = false;
    Vector3 setPosition = Vector3.one;
    Vector3 initialPosition = Vector3.zero;
    float initialValue = 0f;

    public Transform hitPoint;
    int hitPointCallers;
    Vector3 castPos;
    Quaternion castRot;

    //Input system
    struct InputData
    {
        public float drag_delta;
        public float dCont_rotate;
        public Vector2 cnC_thumb;
        public bool cnC_func;
        public bool headConv_conv;
        public float devMode_delta;
        public float direct_func;
        public Quaternion direct_handOri;
        public Vector3 direct_handPos;
        public bool devMode_boost;
        public Vector3 devMode_trans;
    }

    InputData data;

    [SerializeField]
    Transform directionalHand;

    //Method Settings
    public float dragRotationSensitivity;
    public float directionalContinuousRotationSpeed;
    public float paddlingTranslationSpeed;
    public float dragNGoDefaultDistance;


    //Touch related
    struct HandData
    {
        public SimpleHand[] handList;
        public SimpleHand allFingers;
    }

    HandData handData;

    //Drag
    Queue<float> interpolatedDrag = new Queue<float>();

    //Paddling
    Queue<float> interpolatedPaddling = new Queue<float>();

    public RectTransform handInputUI;
    public Text[] fingers;
    int handInputCallers;

    public Transform cncLine;
    public Transform cncWorldLine;

    public bool rotationLocked = false;
    private bool wasRotationLocked = false;
    public bool translationLocked = false;
    private bool wasTranslationLocked = false;

    int castLayerMask;

    //Generalized input

    public void Initialize(MonoBehaviour mono, Action resetAction)
    {
        this.mono = mono;
        this.recenterAction = resetAction;

        dragRotationSensitivity = DataInOut.config.dragRotationSensitivity;
        directionalContinuousRotationSpeed = DataInOut.config.directionalContinuousRotationSpeed;
        paddlingTranslationSpeed = DataInOut.config.paddlingTranslationSpeed;
        dragNGoDefaultDistance = DataInOut.config.dragNGoDefaultDistance;

        handData.handList = new SimpleHand[2];
        handData.handList[0] = new SimpleHand();
        handData.handList[1] = new SimpleHand();

        handInputCallers = 0;
        handInputUI.gameObject.SetActive(false);

        hitPointCallers = 0;
        hitPoint.gameObject.SetActive(false);

        castLayerMask = ~((1 << (LayerMask.NameToLayer("Player"))) | (1 << (LayerMask.NameToLayer("Water"))));

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
        config.R_ClickAndChoose.RotationFunction.started += R_ClickAndChoose_Function;
        config.R_ClickAndChoose.Disable();
        cncLine.parent.gameObject.SetActive(false);
        cncWorldLine.gameObject.SetActive(false);

        //R_Drag
        for (int i = 0; i < DataInOut.config.interpolatedInputFrames; i++)
            interpolatedDrag.Enqueue(0f);

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
        directionalHand.gameObject.SetActive(false);

        //T_Paddling
        for (int i = 0; i < DataInOut.config.interpolatedInputFrames; i++)
            interpolatedPaddling.Enqueue(0f);

    }

    public void HookMonitor(MonitorHandler handler)
    {
        handler.HookConfig(config);
    }

    public void ResolveInput(in Transform headTransform, in Transform controllerTransform)
    {
        CheckforInputSwap();
        if(handInputCallers > 0) ProcessHands();

        ResolveRotation(in headTransform, in controllerTransform);
        ResolveTranslation(in headTransform, in controllerTransform);
    }

    private void CheckforInputSwap()
    {
        if (rotationLocked && !wasRotationLocked)
        {
            switch (rotationType)
            {
                case RotationType.Drag:
                    {
                        if (--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        break;
                    }

                case RotationType.ClickAndChoose:
                    {
                        cncLine.parent.gameObject.SetActive(false);
                        cncWorldLine.gameObject.SetActive(false);
                        break;
                    }

                case RotationType.HeadConverge:
                    {
                        if (--hitPointCallers <= 0) hitPoint.gameObject.SetActive(false);
                        break;
                    }
            }

            wasRotationLocked = true;
        }
        else if(!rotationLocked && wasRotationLocked)
        {
            switch (rotationType)
            {
                case RotationType.Drag:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        break;
                    }

                case RotationType.ClickAndChoose:
                    {
                        //cncLine.parent.gameObject.SetActive(true);
                        //cncWorldLine.gameObject.SetActive(true);
                        break;
                    }

                case RotationType.HeadConverge:
                    {
                        if (++hitPointCallers > 0) hitPoint.gameObject.SetActive(true);
                        break;
                    }
            }

            wasRotationLocked = false;
        }

        if (translationLocked && !wasTranslationLocked)
        {
            switch (translationType)
            {
                case TranslationType.Directional:
                    {
                        directionalHand.gameObject.SetActive(false);
                        break;
                    }

                case TranslationType.DragNGo:
                    {
                        if (--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        if (--hitPointCallers <= 0) hitPoint.gameObject.SetActive(false);
                        break;
                    }

                case TranslationType.Paddling:
                    {
                        if (--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        break;
                    }

            }

            wasTranslationLocked = true;
        }
        else if(!translationLocked && wasTranslationLocked)
        {
            switch (translationType)
            {
                case TranslationType.Directional:
                    {
                        directionalHand.gameObject.SetActive(true);
                        break;
                    }

                case TranslationType.DragNGo:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        if (++hitPointCallers > 0) hitPoint.gameObject.SetActive(true);
                        break;
                    }

                case TranslationType.Paddling:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        break;
                    }

            }

            wasTranslationLocked = false;
        }

        if (rotationType != oldRotationType)
        {
            switch (oldRotationType)
            {
                case RotationType.Drag:
                    {
                        if(--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        break;
                    }

                case RotationType.DirectionalContinuous:
                    {
                        config.R_DirectionalContinuous.Disable();
                        break;
                    }

                case RotationType.ClickAndChoose:
                    {
                        config.R_ClickAndChoose.Disable();
                        cncLine.parent.gameObject.SetActive(false);
                        cncWorldLine.gameObject.SetActive(false);
                        break;
                    }

                case RotationType.HeadConverge:
                    {
                        if (--hitPointCallers <= 0) hitPoint.gameObject.SetActive(false);
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
                case RotationType.Drag:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        break;
                    }

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
                        if (++hitPointCallers > 0) hitPoint.gameObject.SetActive(true);
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
                        directionalHand.gameObject.SetActive(false);
                        break;
                    }

                case TranslationType.DevMode:
                    {
                        config.T_DevMode.Disable();
                        break;
                    }

                case TranslationType.DragNGo:
                    {
                        if (--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        if (--hitPointCallers <= 0) hitPoint.gameObject.SetActive(false);
                        break;
                    }

                case TranslationType.Paddling:
                    {
                        if (--handInputCallers <= 0) handInputUI.gameObject.SetActive(false);
                        break;
                    }

            }

            oldTranslationType = translationType;

            switch (translationType)
            {
                case TranslationType.Directional:
                    {
                        config.T_Directional.Enable();
                        directionalHand.gameObject.SetActive(true);
                        break;
                    }

                case TranslationType.DevMode:
                    {
                        config.T_DevMode.Enable();
                        break;
                    }

                case TranslationType.DragNGo:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        if (++hitPointCallers > 0) hitPoint.gameObject.SetActive(true);
                        break;
                    }

                case TranslationType.Paddling:
                    {
                        if (++handInputCallers > 0) handInputUI.gameObject.SetActive(true);
                        break;
                    }

            }
        }
    }

    void ProcessHands()
    {
        Dictionary<int, SimpleTouch> lostFingers = new Dictionary<int, SimpleTouch>();
        handData.allFingers = new SimpleHand();

        if (Touchscreen.current == null)
        {
            Debug.LogError("Touchscreen hardware appears to be missing! Restart application and try again!");
            return;
        }

        foreach (SimpleHand hand in handData.handList) hand.Decay();

        for (int i = 0; i < Touchscreen.current.touches.Count; i++)
        {
            SimpleTouch t = new SimpleTouch(Touchscreen.current.touches[i], i);

            fingers[i].gameObject.SetActive(false);
            fingers[i].text = "X";

            if (!t.pressed)
            {
                
                foreach (SimpleHand hand in handData.handList)
                {
                    if (hand.ContainsFinger(t.id))
                    {
                        hand.RemoveFinger(t.id);
                        break;
                    }
                }
                continue;
            }

            handData.allFingers[t.id] = t;

            bool foundHand = false;

            foreach (SimpleHand hand in handData.handList)
            {
                if (hand.ContainsFinger(t.id) && hand.Distance(t.position) < 20f)
                {
                    hand[t.id] = t;
                    foundHand = true;
                    break;
                }
            }

            if (!foundHand) lostFingers[t.id] = t;

            
        }

        foreach (SimpleHand hand in handData.handList) hand.CullRipe();

        foreach (KeyValuePair<int, SimpleTouch> finger in lostFingers)
        {
            float bestDist = Mathf.Infinity;
            SimpleHand bestHand = null;

            foreach (SimpleHand hand in handData.handList)
            {
                float dist = hand.Distance(finger.Value.position);
                if (dist < bestDist && dist < 20f)
                {
                    bestDist = dist;
                    bestHand = hand;
                }
            }

            if(bestHand != null)
            {
                bestHand[finger.Key] = finger.Value;
            }
            else
            {
                foreach (SimpleHand hand in handData.handList)
                {
                    if (hand.Count <= 0)
                    {
                        hand[finger.Key] = finger.Value;
                        break;
                    }
                    
                }
            }
        }

        for (int j = 0; j < handData.handList.Length; j++)
        {
            SimpleHand hand = handData.handList[j];
            hand.Update(handInputUI, fingers, j);
        }
    }

    private void ResolveRotation(in Transform headTransform, in Transform controllerTransform)
    {
        switch (rotationType)
        {
            case RotationType.Drag:
                {
                    float x = handData.allFingers.Delta.x;
                    interpolatedDrag.Enqueue(x);

                    float interpolatedX = 0f;

                    foreach (float inX in interpolatedDrag)
                    {
                        interpolatedX += inX;
                    }

                    interpolatedX /= interpolatedDrag.Count;

                    interpolatedDrag.Dequeue();

                    targetRotation = Vector3.up * dragRotationSensitivity * -interpolatedX * Time.unscaledDeltaTime;

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
                    if (data.cnC_thumb.magnitude > 0.5f)
                    {
                        targetOrientation = data.cnC_thumb;
                        wasHolding = true;
                        if(!rotationLocked)
                        {
                            cncLine.parent.gameObject.SetActive(true);
                            cncWorldLine.gameObject.SetActive(true);
                        }
                        cncWorldLine.localRotation = cncLine.localRotation = Quaternion.Euler(0f,0f,-Vector2.SignedAngle(targetOrientation.normalized, Vector2.up));
                    }
                    else if (wasHolding)
                    {
                        wasHolding = false;
                        cncLine.parent.gameObject.SetActive(false);
                        cncWorldLine.gameObject.SetActive(false);
                    }

                    if (data.cnC_func)
                    {
                        if (wasHolding)
                        {
                            float resultInput = Vector2.SignedAngle(targetOrientation.normalized, Vector2.up);
                            if (resultInput != 0f)
                                mono.StartCoroutine(DoRotateStep(resultInput, controllerTransform));

                            targetOrientation = Vector2.up;
                        }
                        data.cnC_func = false;
                    }

                    return;
                }

            case RotationType.HeadConverge:
                {
                    if (translationType == TranslationType.DragNGo && !translationLocked)
                    {
                        data.headConv_conv = handData.handList[1].Tap;
                    }

                    if (data.headConv_conv)
                    {
                        float headAngle = Vector3.SignedAngle(controllerTransform.forward, Vector3.Scale(headTransform.forward, Vector3.one - Vector3.up), controllerTransform.up);

                        if (headAngle != 0f)
                            mono.StartCoroutine(DoRotateStep(headAngle, controllerTransform));

                        data.headConv_conv = false;
                    }

                    if (translationType != TranslationType.DragNGo || translationLocked)
                    {
                        bool hasHit = Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit hit, Mathf.Infinity, castLayerMask);

                        if (hasHit)
                        {
                            castPos = hit.point;
                            castRot = Quaternion.LookRotation(-hit.normal);
                        }
                        else
                        {
                            castPos = headTransform.position + headTransform.forward * dragNGoDefaultDistance;
                            castRot = Quaternion.LookRotation(headTransform.forward);
                        }

                        hitPoint.position = castPos;
                        hitPoint.rotation = castRot;
                        hitPoint.localScale = Vector3.one * (0.1f + Vector3.Distance(headTransform.position, hitPoint.position) / 15f);
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
                    //I really stopped understanding this spaghetti, but for whatever reason, this works perfectly
                    directionalHand.localRotation = data.direct_handOri;
                    targetTranslation = directionalHand.rotation * Vector3.forward * data.direct_func;
                    directionalHand.localPosition = data.direct_handPos;

                    return;
                }

            case TranslationType.Paddling:
                {
                    /*float y = handData.leftHandDelta.y + handData.rightHandDelta.y;
                    if (handData.leftHandTouch && handData.rightHandTouch) y /= 2f;*/

                    Vector2 handDelta;

                    handDelta = handData.allFingers.Delta;

                    Vector3 localForward = headTransform.localRotation * Vector3.forward;

                    Vector2 localHorizontalForward = (new Vector2(localForward.x, localForward.z)).normalized;

                    float y = Vector2.Dot(handDelta, localHorizontalForward);

                    interpolatedPaddling.Enqueue(y);

                    float interpolatedY = 0f;

                    foreach (float inY in interpolatedPaddling)
                    {
                        interpolatedY += inY;
                    }

                    interpolatedY /= interpolatedPaddling.Count;

                    interpolatedPaddling.Dequeue();

                    targetTranslation += headTransform.forward * -interpolatedY * paddlingTranslationSpeed * Time.unscaledDeltaTime;

                    return;
                }

            case TranslationType.DragNGo:
                {
                    float y = 0f;

                    y = handData.handList[0].Position.y;

                    if (isTargetSet)
                    {
                        if (!(handData.handList[0].Touch))
                        {
                            isTargetSet = false;
                        }
                        else
                        {
                            interpolatedDragNGo.Enqueue(y);

                            float interpolatedY = 0f;

                            foreach (float inY in interpolatedDragNGo)
                            {
                                interpolatedY += inY;
                            }

                            interpolatedY /= interpolatedDragNGo.Count;

                            targetPosition = Vector3.LerpUnclamped(setPosition, initialPosition, Mathf.Max(0.05f, interpolatedY / initialValue));
                            if (interpolatedDragNGo.Count > DataInOut.config.interpolatedInputFrames) interpolatedDragNGo.Dequeue();
                        }
                    }

                    if (!isTargetSet)
                    {

                        bool hasHit = Physics.Raycast(headTransform.position, headTransform.forward, out RaycastHit hit, Mathf.Infinity, castLayerMask);

                        if (hasHit)
                        {
                            castPos = hit.point;
                            castRot = Quaternion.LookRotation(-hit.normal);
                        }
                        else
                        {
                            castPos = headTransform.position + headTransform.forward * dragNGoDefaultDistance;
                            castRot = Quaternion.LookRotation(headTransform.forward);
                        }

                        if (handData.handList[0].Touch)
                        {
                            setPosition = castPos;
                            initialPosition = headTransform.position;
                            initialValue = y;
                            isTargetSet = true;
                            interpolatedDragNGo.Clear();
                            interpolatedDragNGo.Enqueue(y);
                        }

                    }

                    hitPoint.position = castPos;
                    hitPoint.rotation = castRot;
                    hitPoint.localScale = Vector3.one * (0.1f + Vector3.Distance(headTransform.position, hitPoint.position) / 15f);

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
        if (translationType == TranslationType.DevMode)
            normalizedResult /= Mathf.Max(1f, targetTranslation.magnitude);

        targetTranslation = Vector3.zero;
        return normalizedResult;
    }

    public Vector3? GetPositionalInput()
    {
        Vector3? result = targetPosition;
        targetPosition = null;
        return result;
    }

    public bool GetTranslationIntent() => targetTranslation.magnitude > 0f || isTargetSet;
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
    void R_ClickAndChoose_Function(InputAction.CallbackContext context) => data.cnC_func = context.ReadValue<float>()>0f;
    void R_HeadConverge_Function(InputAction.CallbackContext context) => data.headConv_conv = context.ReadValue<float>()>0f;
    void T_DevMode_SpeedBoost(InputAction.CallbackContext context) => data.devMode_boost = context.ReadValue<float>()>0f;
    void T_DevMode_TX(InputAction.CallbackContext context) => data.devMode_trans.x = context.ReadValue<float>();
    void T_DevMode_TY(InputAction.CallbackContext context) => data.devMode_trans.y = context.ReadValue<float>();
    void T_DevMode_TZ(InputAction.CallbackContext context) => data.devMode_trans.z = context.ReadValue<float>();
    void T_Directional_Move(InputAction.CallbackContext context)
    {
        data.direct_func = Mathf.Clamp01(context.ReadValue<float>() * 1.05f - 0.025f);
    }

    void T_Directional_Ori(InputAction.CallbackContext context) => data.direct_handOri = context.ReadValue<Quaternion>();
    void T_Directional_Pos(InputAction.CallbackContext context) => data.direct_handPos = context.ReadValue<Vector3>();

}
