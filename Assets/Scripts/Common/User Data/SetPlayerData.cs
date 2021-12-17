using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using UserData;

class SizeOfData
{
    public int[,] userDataArr2;

    public SizeOfData(int x, int y)
    {
        this.userDataArr2 = new int[x, y];
    }
}

public class SetPlayerData : MonoBehaviour
{
    public GameDataManager gameDataManager;      
    public PlayMakerFSM fsm;

    public int _SceneFirstKey;
    public int _CurrentScene;
    public int _Row;

    int key;    
    int eachLastKey;
    int eachFirstKey;

    // 데이터의 인덱싱이 바뀔때 값을 바꿔야 하는 것들 -> TotalFirstKey에 대입값, sOd의 인자 2가지 값
    const int TotalFirstKey = 101;  
    SizeOfData sOd = new SizeOfData(6, 13);    

    public int Row { get => _Row; set => _Row = value; }

    public int CurrentScene { get => _CurrentScene; set => _CurrentScene = value; }

    public int SceneFirstKey { get => _SceneFirstKey; set => _SceneFirstKey = value; }  

    public void SetFirstKeyInScenes(int scene)
    {
       // 씬의 인덱싱이 바뀌면 스위치문의 대입값도 바뀌어야 한다.
        switch (scene)
        {
            case 1: //doorlock
                Row = 0;
                SceneFirstKey = 101;
                CurrentScene = scene;                
                break;
            case 2: //schedule
                Row = 1;
                SceneFirstKey = 201;
                CurrentScene = scene;
                break;
            case 3: //BP_L
                Row = 4;
                SceneFirstKey = 501;
                CurrentScene = scene;
                break;
            case 4: //Scoop L
                Row = 5;
                SceneFirstKey = 601;
                CurrentScene = scene;
                break;
            case 5: //CRUM
                Row = 2;
                SceneFirstKey = 301;
                CurrentScene = scene;
                break;
            case 6: //PlayPaddle
                Row = 3;
                SceneFirstKey = 401;
                CurrentScene = scene;
                break;
            case 7: //bagpacking H
                Row = 4;
                SceneFirstKey = 501;
                CurrentScene = scene;
                break;
            case 8: //Scoop H
                Row = 5;
                SceneFirstKey = 601;
                CurrentScene = scene;
                break;
        }     
    }

    // 진단 Scene의 개수가 바뀌면 아래의 함수 내용을 변경해야 한다.
    private int SetFirstKey(int currentKey)
    {
        if (currentKey <= 199) eachFirstKey = 101;
        else if (currentKey <= 299) eachFirstKey = 201;
        else if (currentKey <= 399) eachFirstKey = 301;
        else if (currentKey <= 499) eachFirstKey = 401;
        else if (currentKey <= 599) eachFirstKey = 501;
        else if (currentKey <= 699) eachFirstKey = 601;
        return eachFirstKey;
    }

    // 각 씬에서 추출하는 데이터의 수가 바뀌면 아래의 eachLastKey 변수를 해당 개수에 맞게 변경해야 한다.
    private int SetLastKey(int currentKey )
    {
        if (currentKey <= 199) eachLastKey = 107;
        else if (currentKey <= 299) eachLastKey = 208;
        else if (currentKey <= 399) eachLastKey = 308;
        else if (currentKey <= 499) eachLastKey = 413;
        else if (currentKey <= 599) eachLastKey = 510;
        else if (currentKey <= 699) eachLastKey = 611;
        return eachLastKey;
    }   

    // 2차원 배열에 모든 키를 셋팅하는 함수
    private int[,] SetEachFirstKey(int[,] arr2)
    {
        int key = TotalFirstKey;                

        for (int i = 0; i < sOd.userDataArr2.GetLength(0); i++)
        {            
            SetLastKey(key);

            for(int j = 0; j < sOd.userDataArr2.GetLength(1); j++)
            {
                sOd.userDataArr2[i, j] = key;
                Debug.Log(sOd.userDataArr2[i, j]);
                if (key == eachLastKey)
                {
                    key -= j;
                    key += 100;
                    break;
                }
                key++;
            }            
        }
        return sOd.userDataArr2;
    }

    public void ClearDataSetting()
    {
        DataManager.GetInstance().dataList.Clear();
    }    

    // 프로그램 시작 후 임의의 진단 Scene이 로드되면 단 한번만 아래의 함수대로 초기화된다
    public void InitialDataSetting()
    {       
        float result = 0;
                
        key = TotalFirstKey;

        for (int i = 1; i <= sOd.userDataArr2.GetLength(0); i++)
        {                           
            SetLastKey(key);
            while (key <= eachLastKey)
            {
                //Debug.Log(key);
                DataManager.GetInstance().dataList.Add(key, new PlayerData("data_" + key, result));
                key++;
            }
            key += 100;

            SetFirstKey(key);
            key = eachFirstKey;
        }               
    }
                            
    // PlayMaker로 값을 전달하는 Scene은 아래의 함수로 오버로드 되어 사용된다
    public void SetSceneData()
    {              
        CurrentScene = SceneManager.GetActiveScene().buildIndex;                        
        SetFirstKeyInScenes(CurrentScene);
        SetEachFirstKey(sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }

        SetLastKey(SceneFirstKey);
        int keyLength = eachLastKey % 10;                

        //mapName 선언
        Dictionary<string, FsmFloat> mapName = new Dictionary<string, FsmFloat>();

        for (int i = 0; i < keyLength; i++)
        {
            int arg0 = sOd.userDataArr2[Row, i];
            mapName.Add(key: string.Format("data_{0}", arg0), 
                value: fsm.FsmVariables.GetFsmFloat("data_" + arg0));

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);

            DataManager.GetInstance().dataList[arg0].Result = mapName["data_" + arg0].Value;            
        }        
    }

    // C# Script로 값을 전달하는 Scene은 아래의 함수로 오버로드 되어 사용된다
    public void SetSceneData(params float[] myVal)
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SetFirstKeyInScenes(CurrentScene);
        SetEachFirstKey(sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }
        
        SetLastKey(SceneFirstKey);
        int keyLength = eachLastKey % 10;
        
        Dictionary<string, float> mapName = new Dictionary<string, float>();

        for (int i = 0; i < keyLength; i++)
        {
            int arg0 = sOd.userDataArr2[Row, i];
            mapName.Add(key: string.Format("data_{0}", arg0), value: myVal[i]);

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);

            DataManager.GetInstance().dataList[arg0].Result = mapName["data_" + arg0];
        }        
    }    
}

