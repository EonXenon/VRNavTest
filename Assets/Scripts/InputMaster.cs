// GENERATED AUTOMATICALLY FROM 'Assets/Settings/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""67fcb6b2-aa21-45f9-9c3d-5e9cc27150bf"",
            ""actions"": [
                {
                    ""name"": ""Recenter"",
                    ""type"": ""Button"",
                    ""id"": ""f2302cac-02da-4e16-a88f-2b48c57024ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation Function"",
                    ""type"": ""Button"",
                    ""id"": ""7201f288-422c-4545-9544-5df75661df0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation Function"",
                    ""type"": ""Button"",
                    ""id"": ""a2a96c10-ee2d-40fb-9421-91c6bf30f8e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation XZ Axis"",
                    ""type"": ""Value"",
                    ""id"": ""ef91dd48-280a-4af8-a679-f3f4e715072f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation Y Axis"",
                    ""type"": ""Value"",
                    ""id"": ""1f43670a-7722-402e-9f08-68f9dda3e40f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Turn Axis"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ed5da7f1-ed02-4185-8d35-a699d333fe5f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Turn Direct"",
                    ""type"": ""Value"",
                    ""id"": ""3599a45f-4af6-4d66-a4b0-8b7549f6caa6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drag Direct"",
                    ""type"": ""PassThrough"",
                    ""id"": ""048aeabd-6d29-4df6-8ddb-c43d617c42b9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AnalogLook"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5be51e5f-e7db-45f7-8b32-40ea7299fe34"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HandOrientation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3e8e51da-590f-4c32-a363-7858b5ff2613"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32655012-a7e9-4f7c-8afa-a24450c514e7"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Recenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e8bfc4d-ebbb-4ca8-8c54-1e780eeb2d31"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerOpenXR>{RightHand}/primarybutton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Recenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c32d6fd-c382-41fd-89a8-820eed913cc6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation Function"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0d2a67b-3b40-4dfd-a99d-65f1be033d9e"",
                    ""path"": ""<OculusTouchController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Translation Function"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8df9267e-d126-47df-bf99-c8c30e2f17ec"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Rotation Function"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7fea13d-193e-4071-8d88-e6b7dab54cb5"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerOpenXR>{RightHand}/triggerpressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Rotation Function"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""690930ca-419b-4c51-89f0-cf2c6dcb0817"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7f23ad93-2fd7-4a2b-a959-1b67a9758225"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9136d9d7-56c3-48fc-9ccc-f1f18f90c9dd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""94739966-e118-4f2a-af38-75e7a785ce0d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d42df858-812d-45bd-a321-774fe8d79252"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""00840d8b-4427-4689-9093-f7792ae27bf5"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerOpenXR>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Translation XZ Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""cd45562d-7639-4b02-be35-dd37e1d9ed35"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d5eae1e6-03e9-4caf-b9c8-39f0257f52ae"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4b22355f-9751-4763-9d06-323237700a7d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""baafcd23-47e7-47d5-95e0-da653986d1bc"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c6e843e0-5c33-4b68-b5d3-123361b30608"",
                    ""path"": ""<OculusTouchController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f898e32d-712f-48cb-af91-9d412ab6695c"",
                    ""path"": ""<OculusTouchController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""80d7a74c-bd83-4c18-8c06-0c7f26083930"",
                    ""path"": ""<OculusTouchController>{RightHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Turn Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""e08ff353-b6f5-44ee-b145-9d80ccdf7a89"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Turn Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0756c125-68b0-488d-9648-db6d4baec21f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Turn Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""aed20876-e87f-4bc2-8441-03b05898c5b2"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Turn Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""be479d39-fe6b-4c8e-941f-7adffc726f32"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Turn Direct"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a0c9c80-0ba0-403d-9be2-d6ecccbef296"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Drag Direct"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""256788fa-996a-4399-b662-00ce1e2f230f"",
                    ""path"": ""<OculusTouchController>{LeftHand}/pointer/velocity/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""Drag Direct"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e60557f1-7d15-45ca-a09f-03e0bb413c5a"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AnalogLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7b02f9b-7b36-4e7d-9190-b0dcc793d800"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerOpenXR>{RightHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""AnalogLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7130ea3b-9c38-402e-98f0-ce9e1f687093"",
                    ""path"": ""<XRController>{LeftHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""VR Controllers"",
                    ""action"": ""HandOrientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""id"": ""88af9920-1506-45f6-bb84-8e8e141b7151"",
            ""actions"": [
                {
                    ""name"": ""Touch"",
                    ""type"": ""Value"",
                    ""id"": ""4d3be37e-52b1-4a11-b2f6-f41c7f2c0d51"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b3269c7f-9924-463f-9720-c8464891c6fd"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch Surface;Keyboard and Mouse;VR Controllers"",
                    ""action"": ""Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch Surface"",
            ""bindingGroup"": ""Touch Surface"",
            ""devices"": []
        },
        {
            ""name"": ""VR Controllers"",
            ""bindingGroup"": ""VR Controllers"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Recenter = m_Player.FindAction("Recenter", throwIfNotFound: true);
        m_Player_TranslationFunction = m_Player.FindAction("Translation Function", throwIfNotFound: true);
        m_Player_RotationFunction = m_Player.FindAction("Rotation Function", throwIfNotFound: true);
        m_Player_TranslationXZAxis = m_Player.FindAction("Translation XZ Axis", throwIfNotFound: true);
        m_Player_TranslationYAxis = m_Player.FindAction("Translation Y Axis", throwIfNotFound: true);
        m_Player_TurnAxis = m_Player.FindAction("Turn Axis", throwIfNotFound: true);
        m_Player_TurnDirect = m_Player.FindAction("Turn Direct", throwIfNotFound: true);
        m_Player_DragDirect = m_Player.FindAction("Drag Direct", throwIfNotFound: true);
        m_Player_AnalogLook = m_Player.FindAction("AnalogLook", throwIfNotFound: true);
        m_Player_HandOrientation = m_Player.FindAction("HandOrientation", throwIfNotFound: true);
        // Touch
        m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
        m_Touch_Touch = m_Touch.FindAction("Touch", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Recenter;
    private readonly InputAction m_Player_TranslationFunction;
    private readonly InputAction m_Player_RotationFunction;
    private readonly InputAction m_Player_TranslationXZAxis;
    private readonly InputAction m_Player_TranslationYAxis;
    private readonly InputAction m_Player_TurnAxis;
    private readonly InputAction m_Player_TurnDirect;
    private readonly InputAction m_Player_DragDirect;
    private readonly InputAction m_Player_AnalogLook;
    private readonly InputAction m_Player_HandOrientation;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Recenter => m_Wrapper.m_Player_Recenter;
        public InputAction @TranslationFunction => m_Wrapper.m_Player_TranslationFunction;
        public InputAction @RotationFunction => m_Wrapper.m_Player_RotationFunction;
        public InputAction @TranslationXZAxis => m_Wrapper.m_Player_TranslationXZAxis;
        public InputAction @TranslationYAxis => m_Wrapper.m_Player_TranslationYAxis;
        public InputAction @TurnAxis => m_Wrapper.m_Player_TurnAxis;
        public InputAction @TurnDirect => m_Wrapper.m_Player_TurnDirect;
        public InputAction @DragDirect => m_Wrapper.m_Player_DragDirect;
        public InputAction @AnalogLook => m_Wrapper.m_Player_AnalogLook;
        public InputAction @HandOrientation => m_Wrapper.m_Player_HandOrientation;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Recenter.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @Recenter.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @Recenter.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRecenter;
                @TranslationFunction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationFunction;
                @TranslationFunction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationFunction;
                @TranslationFunction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationFunction;
                @RotationFunction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotationFunction;
                @RotationFunction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotationFunction;
                @RotationFunction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotationFunction;
                @TranslationXZAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationXZAxis;
                @TranslationXZAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationXZAxis;
                @TranslationXZAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationXZAxis;
                @TranslationYAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationYAxis;
                @TranslationYAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationYAxis;
                @TranslationYAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTranslationYAxis;
                @TurnAxis.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnAxis;
                @TurnAxis.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnAxis;
                @TurnAxis.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnAxis;
                @TurnDirect.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnDirect;
                @TurnDirect.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnDirect;
                @TurnDirect.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTurnDirect;
                @DragDirect.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDragDirect;
                @DragDirect.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDragDirect;
                @DragDirect.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDragDirect;
                @AnalogLook.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogLook;
                @AnalogLook.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogLook;
                @AnalogLook.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAnalogLook;
                @HandOrientation.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHandOrientation;
                @HandOrientation.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHandOrientation;
                @HandOrientation.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHandOrientation;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Recenter.started += instance.OnRecenter;
                @Recenter.performed += instance.OnRecenter;
                @Recenter.canceled += instance.OnRecenter;
                @TranslationFunction.started += instance.OnTranslationFunction;
                @TranslationFunction.performed += instance.OnTranslationFunction;
                @TranslationFunction.canceled += instance.OnTranslationFunction;
                @RotationFunction.started += instance.OnRotationFunction;
                @RotationFunction.performed += instance.OnRotationFunction;
                @RotationFunction.canceled += instance.OnRotationFunction;
                @TranslationXZAxis.started += instance.OnTranslationXZAxis;
                @TranslationXZAxis.performed += instance.OnTranslationXZAxis;
                @TranslationXZAxis.canceled += instance.OnTranslationXZAxis;
                @TranslationYAxis.started += instance.OnTranslationYAxis;
                @TranslationYAxis.performed += instance.OnTranslationYAxis;
                @TranslationYAxis.canceled += instance.OnTranslationYAxis;
                @TurnAxis.started += instance.OnTurnAxis;
                @TurnAxis.performed += instance.OnTurnAxis;
                @TurnAxis.canceled += instance.OnTurnAxis;
                @TurnDirect.started += instance.OnTurnDirect;
                @TurnDirect.performed += instance.OnTurnDirect;
                @TurnDirect.canceled += instance.OnTurnDirect;
                @DragDirect.started += instance.OnDragDirect;
                @DragDirect.performed += instance.OnDragDirect;
                @DragDirect.canceled += instance.OnDragDirect;
                @AnalogLook.started += instance.OnAnalogLook;
                @AnalogLook.performed += instance.OnAnalogLook;
                @AnalogLook.canceled += instance.OnAnalogLook;
                @HandOrientation.started += instance.OnHandOrientation;
                @HandOrientation.performed += instance.OnHandOrientation;
                @HandOrientation.canceled += instance.OnHandOrientation;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Touch
    private readonly InputActionMap m_Touch;
    private ITouchActions m_TouchActionsCallbackInterface;
    private readonly InputAction m_Touch_Touch;
    public struct TouchActions
    {
        private @InputMaster m_Wrapper;
        public TouchActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Touch => m_Wrapper.m_Touch_Touch;
        public InputActionMap Get() { return m_Wrapper.m_Touch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
        public void SetCallbacks(ITouchActions instance)
        {
            if (m_Wrapper.m_TouchActionsCallbackInterface != null)
            {
                @Touch.started -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouch;
                @Touch.performed -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouch;
                @Touch.canceled -= m_Wrapper.m_TouchActionsCallbackInterface.OnTouch;
            }
            m_Wrapper.m_TouchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Touch.started += instance.OnTouch;
                @Touch.performed += instance.OnTouch;
                @Touch.canceled += instance.OnTouch;
            }
        }
    }
    public TouchActions @Touch => new TouchActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_TouchSurfaceSchemeIndex = -1;
    public InputControlScheme TouchSurfaceScheme
    {
        get
        {
            if (m_TouchSurfaceSchemeIndex == -1) m_TouchSurfaceSchemeIndex = asset.FindControlSchemeIndex("Touch Surface");
            return asset.controlSchemes[m_TouchSurfaceSchemeIndex];
        }
    }
    private int m_VRControllersSchemeIndex = -1;
    public InputControlScheme VRControllersScheme
    {
        get
        {
            if (m_VRControllersSchemeIndex == -1) m_VRControllersSchemeIndex = asset.FindControlSchemeIndex("VR Controllers");
            return asset.controlSchemes[m_VRControllersSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnRecenter(InputAction.CallbackContext context);
        void OnTranslationFunction(InputAction.CallbackContext context);
        void OnRotationFunction(InputAction.CallbackContext context);
        void OnTranslationXZAxis(InputAction.CallbackContext context);
        void OnTranslationYAxis(InputAction.CallbackContext context);
        void OnTurnAxis(InputAction.CallbackContext context);
        void OnTurnDirect(InputAction.CallbackContext context);
        void OnDragDirect(InputAction.CallbackContext context);
        void OnAnalogLook(InputAction.CallbackContext context);
        void OnHandOrientation(InputAction.CallbackContext context);
    }
    public interface ITouchActions
    {
        void OnTouch(InputAction.CallbackContext context);
    }
}
