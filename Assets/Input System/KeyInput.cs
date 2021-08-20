using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KetosGames.SceneTransition;

public class KeyInput : MonoBehaviour
{
    private static KeyInput KeyInputInstance;

    private InputMoveScene inputMoveScene;

    private int buildIndex;

    public static KeyInput Instance
    {
        get
        {
            if (KeyInputInstance == null)
            {
                KeyInput keyInput = (KeyInput)GameObject.FindObjectOfType(typeof(KeyInput));
                if (keyInput != null)
                {
                    KeyInputInstance = keyInput;
                }
                else
                {
                    GameObject KeyInputPrefab = Resources.Load<GameObject>("Prefabs/CommonPrefabs/KeyInput");
                    KeyInputInstance = (GameObject.Instantiate(KeyInputPrefab)).GetComponent<KeyInput>();
                }
            }
            return KeyInputInstance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);


        if (KeyInputInstance != null && KeyInputInstance != this)
        {
            Destroy(KeyInputInstance.gameObject);
            KeyInputInstance = this;
        }
        inputMoveScene = new InputMoveScene();      
    }
    
    private void OnEnable()
    {        
        inputMoveScene.SceneCotrolMap.MoveToScene_1.performed += GoToScene_1;
        inputMoveScene.SceneCotrolMap.MoveToScene_2.performed += GoToScene_2;
        inputMoveScene.SceneCotrolMap.MoveToScene_3.performed += GoToScene_3;
        inputMoveScene.SceneCotrolMap.MoveToScene_4.performed += GoToScene_4;
        inputMoveScene.SceneCotrolMap.MoveToScene_5.performed += GoToScene_5;
        inputMoveScene.SceneCotrolMap.MoveToScene_6.performed += GoToScene_6;
        inputMoveScene.SceneCotrolMap.MoveToScene_7.performed += GoToScene_7;
        inputMoveScene.SceneCotrolMap.MoveToScene_8.performed += GoToScene_8;
        inputMoveScene.SceneCotrolMap.MoveToScene_9.performed += GoToScene_9;
        inputMoveScene.SceneCotrolMap.MoveToScene_10.performed += GoToScene_10;

        inputMoveScene.SceneCotrolMap.MoveToScene_1.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_2.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_3.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_4.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_5.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_6.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_7.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_8.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_9.Enable();
        inputMoveScene.SceneCotrolMap.MoveToScene_10.Enable();
    }

    private void OnDisable()
    {
        inputMoveScene.SceneCotrolMap.MoveToScene_1.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_2.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_3.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_4.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_5.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_6.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_7.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_8.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_9.Disable();
        inputMoveScene.SceneCotrolMap.MoveToScene_10.Disable();
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
    private void GoToScene_10(InputAction.CallbackContext obj)
    {
        // input scene으로 가게 할 예정임 (몇 가지 문제가 있어서 해결이 되면 가능)
        buildIndex = 0;
        SceneLoader.LoadScene(buildIndex);
    }
}
