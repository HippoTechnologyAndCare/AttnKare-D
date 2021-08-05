// GENERATED AUTOMATICALLY FROM 'Assets/Input System/InputMoveScene.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMoveScene : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMoveScene()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMoveScene"",
    ""maps"": [
        {
            ""name"": ""SceneCotrolMap"",
            ""id"": ""1bf1c08a-9cb0-4dc3-ae17-9e5e0db58ad8"",
            ""actions"": [
                {
                    ""name"": ""MoveToScene"",
                    ""type"": ""Value"",
                    ""id"": ""3601b8c3-3f10-41c7-afb0-13fbc4096f64"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""94f179af-a5f1-432f-a349-20b89784cbb1"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a224e67d-8c34-49db-b4a4-20ab035e69ee"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5965d13b-d6cf-4ab2-8060-e6fb54df21ce"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a60adc30-255c-491e-bf51-71ee4a4d6ec7"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""384a874b-b891-4863-a7a2-c4a510217bc0"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a8a1b0d-9591-4ba4-9ca5-4e7b406b25d6"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fc3b66a-3fc8-43a7-bf5a-52153d9cc962"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91180a99-8c4f-439f-9872-42a99eb60623"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ca6ff43-b7ae-4aa6-a36e-325bba461a57"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19cdfc2d-c17f-4428-9488-395020c75bd9"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6521e6a-db27-4773-b996-f9385c2679ed"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MoveToScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR Cotroller"",
            ""bindingGroup"": ""XR Cotroller"",
            ""devices"": [
                {
                    ""devicePath"": ""<OculusTouchController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // SceneCotrolMap
        m_SceneCotrolMap = asset.FindActionMap("SceneCotrolMap", throwIfNotFound: true);
        m_SceneCotrolMap_MoveToScene = m_SceneCotrolMap.FindAction("MoveToScene", throwIfNotFound: true);
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

    // SceneCotrolMap
    private readonly InputActionMap m_SceneCotrolMap;
    private ISceneCotrolMapActions m_SceneCotrolMapActionsCallbackInterface;
    private readonly InputAction m_SceneCotrolMap_MoveToScene;
    public struct SceneCotrolMapActions
    {
        private @InputMoveScene m_Wrapper;
        public SceneCotrolMapActions(@InputMoveScene wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveToScene => m_Wrapper.m_SceneCotrolMap_MoveToScene;
        public InputActionMap Get() { return m_Wrapper.m_SceneCotrolMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SceneCotrolMapActions set) { return set.Get(); }
        public void SetCallbacks(ISceneCotrolMapActions instance)
        {
            if (m_Wrapper.m_SceneCotrolMapActionsCallbackInterface != null)
            {
                @MoveToScene.started -= m_Wrapper.m_SceneCotrolMapActionsCallbackInterface.OnMoveToScene;
                @MoveToScene.performed -= m_Wrapper.m_SceneCotrolMapActionsCallbackInterface.OnMoveToScene;
                @MoveToScene.canceled -= m_Wrapper.m_SceneCotrolMapActionsCallbackInterface.OnMoveToScene;
            }
            m_Wrapper.m_SceneCotrolMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveToScene.started += instance.OnMoveToScene;
                @MoveToScene.performed += instance.OnMoveToScene;
                @MoveToScene.canceled += instance.OnMoveToScene;
            }
        }
    }
    public SceneCotrolMapActions @SceneCotrolMap => new SceneCotrolMapActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_XRCotrollerSchemeIndex = -1;
    public InputControlScheme XRCotrollerScheme
    {
        get
        {
            if (m_XRCotrollerSchemeIndex == -1) m_XRCotrollerSchemeIndex = asset.FindControlSchemeIndex("XR Cotroller");
            return asset.controlSchemes[m_XRCotrollerSchemeIndex];
        }
    }
    public interface ISceneCotrolMapActions
    {
        void OnMoveToScene(InputAction.CallbackContext context);
    }
}
