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
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9e8bfc4d-ebbb-4ca8-8c54-1e780eeb2d31"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Recenter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""R_DirectionalContinuous"",
            ""id"": ""ff942b1e-9a9a-4e63-9770-08acce94f05c"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f4ff4a36-0c75-42f3-b658-3f7cb9997bf0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7583c655-a375-437b-8fb3-e45f36b28c87"",
                    ""path"": ""<XRController>{LeftHand}/thumbstick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""R_ClickAndChoose"",
            ""id"": ""b6d58b59-41c4-4af2-9cd3-1cdad6dc93fe"",
            ""actions"": [
                {
                    ""name"": ""Direction"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5c983441-e019-4fa1-a923-52954258e006"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d7a2acda-182a-4fc4-920b-cbce413ad186"",
                    ""path"": ""<XRController>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""R_HeadConverge"",
            ""id"": ""a625f997-fbfe-473d-a85b-5e2e95a7dac7"",
            ""actions"": [
                {
                    ""name"": ""Rotation Function"",
                    ""type"": ""Button"",
                    ""id"": ""0d1e7c27-a194-4d7c-93ce-ca70c67f6caa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c7b3795b-6951-447a-9b31-6db2c0ce8d0c"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation Function"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""R_DevMode"",
            ""id"": ""3d7b5b3b-ec71-4115-b0bc-05b1b29907a4"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5886205c-0bcb-45c3-8e68-508a4a6e2e9d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cff68692-83e5-417d-a1b0-7c743dbe8496"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""T_Directional"",
            ""id"": ""7f3b2bd5-91f0-43ba-b8f1-457832c9913e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8ebff63d-4238-4a40-a1d5-b916d4a9b1d2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hand Orientation"",
                    ""type"": ""Value"",
                    ""id"": ""3ed1dc9d-b3d2-444d-a464-e6ab7ab20102"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Hand Position"",
                    ""type"": ""Value"",
                    ""id"": ""5311ef21-460f-4d60-843f-21d0f04f9fec"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""da8f3660-eff7-4081-b53b-403c05053408"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77e8e530-903b-4699-8db0-7073759b73ab"",
                    ""path"": ""<XRController>{LeftHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hand Orientation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b103f794-b025-4ef5-bc33-de4903671f84"",
                    ""path"": ""<XRController>{LeftHand}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hand Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""T_DevMode"",
            ""id"": ""c4ca6ba0-5e42-4aa5-b453-b43e4ee39622"",
            ""actions"": [
                {
                    ""name"": ""Speed Boost"",
                    ""type"": ""Button"",
                    ""id"": ""9f2afb6e-20bb-4fd5-9069-7992fdbda30c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation X Axis"",
                    ""type"": ""Value"",
                    ""id"": ""bdab6c47-4071-4ca2-a7ef-fdbe126544d8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation Y Axis"",
                    ""type"": ""Value"",
                    ""id"": ""5c12d18c-8918-4500-bd11-10ed081bb357"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Translation Z Axis"",
                    ""type"": ""Value"",
                    ""id"": ""421925c8-e403-4c26-81d7-1f0c11b34bea"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""94668135-e7fd-445d-9594-99abb5239736"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation X Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""60bd9fef-5314-4b38-8172-a624366faada"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation X Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c45bae8a-8ea9-4bc1-870b-de37007fda9a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation X Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""46231dd0-2dc2-443f-a1ab-135327eb924c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Z Axis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6f062192-74f7-4feb-b9fb-61e0bfcdb345"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Z Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""051c2123-42a7-4984-96de-8ca8218b04d9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Z Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""3f8e6390-763b-43b7-82c2-f06f8f532096"",
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
                    ""id"": ""3e65f159-3826-4332-a88b-3a5526efe600"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""19de15b7-190a-4c6b-bd14-426790313572"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation Y Axis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""54eff0e6-c4ef-4400-a114-eb49b4f8c0fc"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Speed Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Monitor"",
            ""id"": ""b5299d7f-3aae-4984-91ce-7057ead138b7"",
            ""actions"": [
                {
                    ""name"": ""MenuUp"",
                    ""type"": ""Button"",
                    ""id"": ""f648dccf-e820-4a2e-8921-b8ffc6e1821f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuDown"",
                    ""type"": ""Button"",
                    ""id"": ""1891a6d3-2fce-48e1-87ba-3c93f73de89b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuLeft"",
                    ""type"": ""Button"",
                    ""id"": ""1b9be84c-aad4-4ad9-854f-cf7249bbd97b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuRight"",
                    ""type"": ""Button"",
                    ""id"": ""89f0ac5e-259e-45b3-a291-7a8b922aa95e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuSelect"",
                    ""type"": ""Button"",
                    ""id"": ""8370cda5-8a0d-4234-9e45-ca2e93b3e830"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ExitGame"",
                    ""type"": ""Button"",
                    ""id"": ""a02872b4-a41f-4cc3-9174-23c38a97e74c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f7d2b6b9-9ca5-47e8-ad2f-717fe099a89e"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83ecaa7b-57c3-4791-980a-0ebfc09ac104"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f9474af-5532-4768-b15b-15896d6efb05"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a676ca0a-9189-4500-9646-3975990c63d3"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26b04622-55d8-46fa-82ad-f2edc1b8f73d"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f07b3e5d-8258-4924-acc3-ff6c8251ed73"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default Control Scheme"",
            ""bindingGroup"": ""Default Control Scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Recenter = m_Player.FindAction("Recenter", throwIfNotFound: true);
        // R_DirectionalContinuous
        m_R_DirectionalContinuous = asset.FindActionMap("R_DirectionalContinuous", throwIfNotFound: true);
        m_R_DirectionalContinuous_Rotate = m_R_DirectionalContinuous.FindAction("Rotate", throwIfNotFound: true);
        // R_ClickAndChoose
        m_R_ClickAndChoose = asset.FindActionMap("R_ClickAndChoose", throwIfNotFound: true);
        m_R_ClickAndChoose_Direction = m_R_ClickAndChoose.FindAction("Direction", throwIfNotFound: true);
        // R_HeadConverge
        m_R_HeadConverge = asset.FindActionMap("R_HeadConverge", throwIfNotFound: true);
        m_R_HeadConverge_RotationFunction = m_R_HeadConverge.FindAction("Rotation Function", throwIfNotFound: true);
        // R_DevMode
        m_R_DevMode = asset.FindActionMap("R_DevMode", throwIfNotFound: true);
        m_R_DevMode_Rotate = m_R_DevMode.FindAction("Rotate", throwIfNotFound: true);
        // T_Directional
        m_T_Directional = asset.FindActionMap("T_Directional", throwIfNotFound: true);
        m_T_Directional_Move = m_T_Directional.FindAction("Move", throwIfNotFound: true);
        m_T_Directional_HandOrientation = m_T_Directional.FindAction("Hand Orientation", throwIfNotFound: true);
        m_T_Directional_HandPosition = m_T_Directional.FindAction("Hand Position", throwIfNotFound: true);
        // T_DevMode
        m_T_DevMode = asset.FindActionMap("T_DevMode", throwIfNotFound: true);
        m_T_DevMode_SpeedBoost = m_T_DevMode.FindAction("Speed Boost", throwIfNotFound: true);
        m_T_DevMode_TranslationXAxis = m_T_DevMode.FindAction("Translation X Axis", throwIfNotFound: true);
        m_T_DevMode_TranslationYAxis = m_T_DevMode.FindAction("Translation Y Axis", throwIfNotFound: true);
        m_T_DevMode_TranslationZAxis = m_T_DevMode.FindAction("Translation Z Axis", throwIfNotFound: true);
        // Monitor
        m_Monitor = asset.FindActionMap("Monitor", throwIfNotFound: true);
        m_Monitor_MenuUp = m_Monitor.FindAction("MenuUp", throwIfNotFound: true);
        m_Monitor_MenuDown = m_Monitor.FindAction("MenuDown", throwIfNotFound: true);
        m_Monitor_MenuLeft = m_Monitor.FindAction("MenuLeft", throwIfNotFound: true);
        m_Monitor_MenuRight = m_Monitor.FindAction("MenuRight", throwIfNotFound: true);
        m_Monitor_MenuSelect = m_Monitor.FindAction("MenuSelect", throwIfNotFound: true);
        m_Monitor_ExitGame = m_Monitor.FindAction("ExitGame", throwIfNotFound: true);
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
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Recenter => m_Wrapper.m_Player_Recenter;
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
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Recenter.started += instance.OnRecenter;
                @Recenter.performed += instance.OnRecenter;
                @Recenter.canceled += instance.OnRecenter;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // R_DirectionalContinuous
    private readonly InputActionMap m_R_DirectionalContinuous;
    private IR_DirectionalContinuousActions m_R_DirectionalContinuousActionsCallbackInterface;
    private readonly InputAction m_R_DirectionalContinuous_Rotate;
    public struct R_DirectionalContinuousActions
    {
        private @InputMaster m_Wrapper;
        public R_DirectionalContinuousActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_R_DirectionalContinuous_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_R_DirectionalContinuous; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(R_DirectionalContinuousActions set) { return set.Get(); }
        public void SetCallbacks(IR_DirectionalContinuousActions instance)
        {
            if (m_Wrapper.m_R_DirectionalContinuousActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_R_DirectionalContinuousActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_R_DirectionalContinuousActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_R_DirectionalContinuousActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_R_DirectionalContinuousActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public R_DirectionalContinuousActions @R_DirectionalContinuous => new R_DirectionalContinuousActions(this);

    // R_ClickAndChoose
    private readonly InputActionMap m_R_ClickAndChoose;
    private IR_ClickAndChooseActions m_R_ClickAndChooseActionsCallbackInterface;
    private readonly InputAction m_R_ClickAndChoose_Direction;
    public struct R_ClickAndChooseActions
    {
        private @InputMaster m_Wrapper;
        public R_ClickAndChooseActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Direction => m_Wrapper.m_R_ClickAndChoose_Direction;
        public InputActionMap Get() { return m_Wrapper.m_R_ClickAndChoose; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(R_ClickAndChooseActions set) { return set.Get(); }
        public void SetCallbacks(IR_ClickAndChooseActions instance)
        {
            if (m_Wrapper.m_R_ClickAndChooseActionsCallbackInterface != null)
            {
                @Direction.started -= m_Wrapper.m_R_ClickAndChooseActionsCallbackInterface.OnDirection;
                @Direction.performed -= m_Wrapper.m_R_ClickAndChooseActionsCallbackInterface.OnDirection;
                @Direction.canceled -= m_Wrapper.m_R_ClickAndChooseActionsCallbackInterface.OnDirection;
            }
            m_Wrapper.m_R_ClickAndChooseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Direction.started += instance.OnDirection;
                @Direction.performed += instance.OnDirection;
                @Direction.canceled += instance.OnDirection;
            }
        }
    }
    public R_ClickAndChooseActions @R_ClickAndChoose => new R_ClickAndChooseActions(this);

    // R_HeadConverge
    private readonly InputActionMap m_R_HeadConverge;
    private IR_HeadConvergeActions m_R_HeadConvergeActionsCallbackInterface;
    private readonly InputAction m_R_HeadConverge_RotationFunction;
    public struct R_HeadConvergeActions
    {
        private @InputMaster m_Wrapper;
        public R_HeadConvergeActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotationFunction => m_Wrapper.m_R_HeadConverge_RotationFunction;
        public InputActionMap Get() { return m_Wrapper.m_R_HeadConverge; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(R_HeadConvergeActions set) { return set.Get(); }
        public void SetCallbacks(IR_HeadConvergeActions instance)
        {
            if (m_Wrapper.m_R_HeadConvergeActionsCallbackInterface != null)
            {
                @RotationFunction.started -= m_Wrapper.m_R_HeadConvergeActionsCallbackInterface.OnRotationFunction;
                @RotationFunction.performed -= m_Wrapper.m_R_HeadConvergeActionsCallbackInterface.OnRotationFunction;
                @RotationFunction.canceled -= m_Wrapper.m_R_HeadConvergeActionsCallbackInterface.OnRotationFunction;
            }
            m_Wrapper.m_R_HeadConvergeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RotationFunction.started += instance.OnRotationFunction;
                @RotationFunction.performed += instance.OnRotationFunction;
                @RotationFunction.canceled += instance.OnRotationFunction;
            }
        }
    }
    public R_HeadConvergeActions @R_HeadConverge => new R_HeadConvergeActions(this);

    // R_DevMode
    private readonly InputActionMap m_R_DevMode;
    private IR_DevModeActions m_R_DevModeActionsCallbackInterface;
    private readonly InputAction m_R_DevMode_Rotate;
    public struct R_DevModeActions
    {
        private @InputMaster m_Wrapper;
        public R_DevModeActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_R_DevMode_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_R_DevMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(R_DevModeActions set) { return set.Get(); }
        public void SetCallbacks(IR_DevModeActions instance)
        {
            if (m_Wrapper.m_R_DevModeActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_R_DevModeActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_R_DevModeActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_R_DevModeActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_R_DevModeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public R_DevModeActions @R_DevMode => new R_DevModeActions(this);

    // T_Directional
    private readonly InputActionMap m_T_Directional;
    private IT_DirectionalActions m_T_DirectionalActionsCallbackInterface;
    private readonly InputAction m_T_Directional_Move;
    private readonly InputAction m_T_Directional_HandOrientation;
    private readonly InputAction m_T_Directional_HandPosition;
    public struct T_DirectionalActions
    {
        private @InputMaster m_Wrapper;
        public T_DirectionalActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_T_Directional_Move;
        public InputAction @HandOrientation => m_Wrapper.m_T_Directional_HandOrientation;
        public InputAction @HandPosition => m_Wrapper.m_T_Directional_HandPosition;
        public InputActionMap Get() { return m_Wrapper.m_T_Directional; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(T_DirectionalActions set) { return set.Get(); }
        public void SetCallbacks(IT_DirectionalActions instance)
        {
            if (m_Wrapper.m_T_DirectionalActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnMove;
                @HandOrientation.started -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandOrientation;
                @HandOrientation.performed -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandOrientation;
                @HandOrientation.canceled -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandOrientation;
                @HandPosition.started -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandPosition;
                @HandPosition.performed -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandPosition;
                @HandPosition.canceled -= m_Wrapper.m_T_DirectionalActionsCallbackInterface.OnHandPosition;
            }
            m_Wrapper.m_T_DirectionalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @HandOrientation.started += instance.OnHandOrientation;
                @HandOrientation.performed += instance.OnHandOrientation;
                @HandOrientation.canceled += instance.OnHandOrientation;
                @HandPosition.started += instance.OnHandPosition;
                @HandPosition.performed += instance.OnHandPosition;
                @HandPosition.canceled += instance.OnHandPosition;
            }
        }
    }
    public T_DirectionalActions @T_Directional => new T_DirectionalActions(this);

    // T_DevMode
    private readonly InputActionMap m_T_DevMode;
    private IT_DevModeActions m_T_DevModeActionsCallbackInterface;
    private readonly InputAction m_T_DevMode_SpeedBoost;
    private readonly InputAction m_T_DevMode_TranslationXAxis;
    private readonly InputAction m_T_DevMode_TranslationYAxis;
    private readonly InputAction m_T_DevMode_TranslationZAxis;
    public struct T_DevModeActions
    {
        private @InputMaster m_Wrapper;
        public T_DevModeActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpeedBoost => m_Wrapper.m_T_DevMode_SpeedBoost;
        public InputAction @TranslationXAxis => m_Wrapper.m_T_DevMode_TranslationXAxis;
        public InputAction @TranslationYAxis => m_Wrapper.m_T_DevMode_TranslationYAxis;
        public InputAction @TranslationZAxis => m_Wrapper.m_T_DevMode_TranslationZAxis;
        public InputActionMap Get() { return m_Wrapper.m_T_DevMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(T_DevModeActions set) { return set.Get(); }
        public void SetCallbacks(IT_DevModeActions instance)
        {
            if (m_Wrapper.m_T_DevModeActionsCallbackInterface != null)
            {
                @SpeedBoost.started -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnSpeedBoost;
                @SpeedBoost.performed -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnSpeedBoost;
                @SpeedBoost.canceled -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnSpeedBoost;
                @TranslationXAxis.started -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationXAxis;
                @TranslationXAxis.performed -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationXAxis;
                @TranslationXAxis.canceled -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationXAxis;
                @TranslationYAxis.started -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationYAxis;
                @TranslationYAxis.performed -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationYAxis;
                @TranslationYAxis.canceled -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationYAxis;
                @TranslationZAxis.started -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationZAxis;
                @TranslationZAxis.performed -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationZAxis;
                @TranslationZAxis.canceled -= m_Wrapper.m_T_DevModeActionsCallbackInterface.OnTranslationZAxis;
            }
            m_Wrapper.m_T_DevModeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SpeedBoost.started += instance.OnSpeedBoost;
                @SpeedBoost.performed += instance.OnSpeedBoost;
                @SpeedBoost.canceled += instance.OnSpeedBoost;
                @TranslationXAxis.started += instance.OnTranslationXAxis;
                @TranslationXAxis.performed += instance.OnTranslationXAxis;
                @TranslationXAxis.canceled += instance.OnTranslationXAxis;
                @TranslationYAxis.started += instance.OnTranslationYAxis;
                @TranslationYAxis.performed += instance.OnTranslationYAxis;
                @TranslationYAxis.canceled += instance.OnTranslationYAxis;
                @TranslationZAxis.started += instance.OnTranslationZAxis;
                @TranslationZAxis.performed += instance.OnTranslationZAxis;
                @TranslationZAxis.canceled += instance.OnTranslationZAxis;
            }
        }
    }
    public T_DevModeActions @T_DevMode => new T_DevModeActions(this);

    // Monitor
    private readonly InputActionMap m_Monitor;
    private IMonitorActions m_MonitorActionsCallbackInterface;
    private readonly InputAction m_Monitor_MenuUp;
    private readonly InputAction m_Monitor_MenuDown;
    private readonly InputAction m_Monitor_MenuLeft;
    private readonly InputAction m_Monitor_MenuRight;
    private readonly InputAction m_Monitor_MenuSelect;
    private readonly InputAction m_Monitor_ExitGame;
    public struct MonitorActions
    {
        private @InputMaster m_Wrapper;
        public MonitorActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuUp => m_Wrapper.m_Monitor_MenuUp;
        public InputAction @MenuDown => m_Wrapper.m_Monitor_MenuDown;
        public InputAction @MenuLeft => m_Wrapper.m_Monitor_MenuLeft;
        public InputAction @MenuRight => m_Wrapper.m_Monitor_MenuRight;
        public InputAction @MenuSelect => m_Wrapper.m_Monitor_MenuSelect;
        public InputAction @ExitGame => m_Wrapper.m_Monitor_ExitGame;
        public InputActionMap Get() { return m_Wrapper.m_Monitor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MonitorActions set) { return set.Get(); }
        public void SetCallbacks(IMonitorActions instance)
        {
            if (m_Wrapper.m_MonitorActionsCallbackInterface != null)
            {
                @MenuUp.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuUp;
                @MenuUp.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuUp;
                @MenuUp.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuUp;
                @MenuDown.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuDown;
                @MenuDown.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuDown;
                @MenuDown.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuDown;
                @MenuLeft.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuLeft;
                @MenuLeft.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuLeft;
                @MenuLeft.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuLeft;
                @MenuRight.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuRight;
                @MenuRight.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuRight;
                @MenuRight.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuRight;
                @MenuSelect.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuSelect;
                @MenuSelect.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuSelect;
                @MenuSelect.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnMenuSelect;
                @ExitGame.started -= m_Wrapper.m_MonitorActionsCallbackInterface.OnExitGame;
                @ExitGame.performed -= m_Wrapper.m_MonitorActionsCallbackInterface.OnExitGame;
                @ExitGame.canceled -= m_Wrapper.m_MonitorActionsCallbackInterface.OnExitGame;
            }
            m_Wrapper.m_MonitorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MenuUp.started += instance.OnMenuUp;
                @MenuUp.performed += instance.OnMenuUp;
                @MenuUp.canceled += instance.OnMenuUp;
                @MenuDown.started += instance.OnMenuDown;
                @MenuDown.performed += instance.OnMenuDown;
                @MenuDown.canceled += instance.OnMenuDown;
                @MenuLeft.started += instance.OnMenuLeft;
                @MenuLeft.performed += instance.OnMenuLeft;
                @MenuLeft.canceled += instance.OnMenuLeft;
                @MenuRight.started += instance.OnMenuRight;
                @MenuRight.performed += instance.OnMenuRight;
                @MenuRight.canceled += instance.OnMenuRight;
                @MenuSelect.started += instance.OnMenuSelect;
                @MenuSelect.performed += instance.OnMenuSelect;
                @MenuSelect.canceled += instance.OnMenuSelect;
                @ExitGame.started += instance.OnExitGame;
                @ExitGame.performed += instance.OnExitGame;
                @ExitGame.canceled += instance.OnExitGame;
            }
        }
    }
    public MonitorActions @Monitor => new MonitorActions(this);
    private int m_DefaultControlSchemeSchemeIndex = -1;
    public InputControlScheme DefaultControlSchemeScheme
    {
        get
        {
            if (m_DefaultControlSchemeSchemeIndex == -1) m_DefaultControlSchemeSchemeIndex = asset.FindControlSchemeIndex("Default Control Scheme");
            return asset.controlSchemes[m_DefaultControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnRecenter(InputAction.CallbackContext context);
    }
    public interface IR_DirectionalContinuousActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IR_ClickAndChooseActions
    {
        void OnDirection(InputAction.CallbackContext context);
    }
    public interface IR_HeadConvergeActions
    {
        void OnRotationFunction(InputAction.CallbackContext context);
    }
    public interface IR_DevModeActions
    {
        void OnRotate(InputAction.CallbackContext context);
    }
    public interface IT_DirectionalActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnHandOrientation(InputAction.CallbackContext context);
        void OnHandPosition(InputAction.CallbackContext context);
    }
    public interface IT_DevModeActions
    {
        void OnSpeedBoost(InputAction.CallbackContext context);
        void OnTranslationXAxis(InputAction.CallbackContext context);
        void OnTranslationYAxis(InputAction.CallbackContext context);
        void OnTranslationZAxis(InputAction.CallbackContext context);
    }
    public interface IMonitorActions
    {
        void OnMenuUp(InputAction.CallbackContext context);
        void OnMenuDown(InputAction.CallbackContext context);
        void OnMenuLeft(InputAction.CallbackContext context);
        void OnMenuRight(InputAction.CallbackContext context);
        void OnMenuSelect(InputAction.CallbackContext context);
        void OnExitGame(InputAction.CallbackContext context);
    }
}
