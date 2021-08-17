using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KetosGames.SceneTransition;

public class KeyInput : MonoBehaviour
{  
    private InputMoveScene inputMoveScene;

    static public KeyInput instance;

    private int buildIndex;
    private int Length;
    private bool isOverlapped;

    private void Awake()
    {
        var objs = FindObjectOfType<KeyInput>();

        if (objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        inputMoveScene = new InputMoveScene();      
    }
    
    private void OnEnable()
    {        
        inputMoveScene.SceneControlMap.MoveToScene_1.performed += GoToScene_1;
        inputMoveScene.SceneControlMap.MoveToScene_2.performed += GoToScene_2;
        inputMoveScene.SceneControlMap.MoveToScene_3.performed += GoToScene_3;
        inputMoveScene.SceneControlMap.MoveToScene_4.performed += GoToScene_4;
        inputMoveScene.SceneControlMap.MoveToScene_5.performed += GoToScene_5;
        inputMoveScene.SceneControlMap.MoveToScene_6.performed += GoToScene_6;
        inputMoveScene.SceneControlMap.MoveToScene_7.performed += GoToScene_7;
        inputMoveScene.SceneControlMap.MoveToScene_8.performed += GoToScene_8;
        inputMoveScene.SceneControlMap.MoveToScene_9.performed += GoToScene_9;
        // 사용 안함
        //inputMoveScene.SceneControlMap.MoveToScene_10.performed += GoToScene_10;

        inputMoveScene.SceneControlMap.MoveToScene_1.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_2.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_3.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_4.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_5.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_6.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_7.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_8.Enable();
        inputMoveScene.SceneControlMap.MoveToScene_9.Enable();
        // 사용 안함
        //inputMoveScene.SceneControlMap.MoveToScene_10.Enable();
    }

    private void OnDisable()
    {
        inputMoveScene.SceneControlMap.MoveToScene_1.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_2.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_3.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_4.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_5.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_6.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_7.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_8.Disable();
        inputMoveScene.SceneControlMap.MoveToScene_9.Disable();
        // 사용 안함
        //inputMoveScene.SceneControlMap.MoveToScene_10.Disable();
    }

    private void GoToScene_1(InputAction.CallbackContext obj)
    {
        buildIndex = 1;
        SceneLoader.LoadScene(buildIndex);
    }

    private void GoToScene_2(InputAction.CallbackContext obj)
    {
        buildIndex = 2;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_3(InputAction.CallbackContext obj)
    {
        buildIndex = 3;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_4(InputAction.CallbackContext obj)
    {
        buildIndex = 4;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_5(InputAction.CallbackContext obj)
    {
        buildIndex = 5;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_6(InputAction.CallbackContext obj)
    {
        buildIndex = 6;
        SceneLoader.LoadScene(buildIndex);
    }

    private void GoToScene_7(InputAction.CallbackContext obj)
    {
        buildIndex = 7;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_8(InputAction.CallbackContext obj)
    {
        buildIndex = 8;
        SceneLoader.LoadScene(buildIndex);
    }
    private void GoToScene_9(InputAction.CallbackContext obj)
    {
        buildIndex = 9;
        SceneLoader.LoadScene(buildIndex);
    }

    // 사용 안함
    //private void GoToScene_10(InputAction.CallbackContext obj)
    //{
    //    buildIndex = 10;
    //    SceneLoader.LoadScene(buildIndex);
    //}
}
