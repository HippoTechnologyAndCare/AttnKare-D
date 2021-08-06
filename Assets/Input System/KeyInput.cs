using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class KeyInput : MonoBehaviour
{
    public InputMoveScene controls;

    void Awake()
    {
        controls.SceneCotrolMap.MoveToScene.performed += _ => MoveToScene();
    }

    void MoveToScene()
    {
        Debug.Log("We move to the Scene");
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
