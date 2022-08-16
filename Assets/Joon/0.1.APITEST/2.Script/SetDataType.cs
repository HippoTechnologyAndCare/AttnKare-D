using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;
using UserData;

class DataSize
{
    public int[,] userDataArr2;

    public DataSize(int x, int y)
    {
        this.userDataArr2 = new int[x, y];
    }
}
public class PlayerJsonData // List<T>�� �� Ŭ���� ����
{
    // PlayerData�� �ٷ� ����Ʈ�� �� �Ű������� ���� �͵��� �̸� ������ ������ �д�        
    public string DataName;
    public object Result;

    // ������ ������
    public PlayerJsonData(string dataName, object result)
    {
        this.DataName = dataName;
        this.Result = result;
    }
}

public class SetDataType : MonoBehaviour
{
    public PlayMakerFSM fsm;

    public int _SceneFirstKey;
    public int _CurrentScene;
    public int _Row;

    int key;
    string m_skey;
    int FirstKey;
    int m_nFirstKey;
    int m_nLastKey;
    int eachLastKey;
    int eachFirstKey;
    int keyLength;

    // �������� �ε����� �ٲ� ���� �ٲ�� �ϴ� �͵� -> TotalFirstKey�� ���԰�, sOd�� ���� 2���� ��
    const int TotalFirstKey = 101;
    public JsonDataManager JsonData;
    DataSize sOd = new DataSize(9, 21);

    public int Row { get => _Row; set => _Row = value; }

    public int CurrentScene { get => _CurrentScene; set => _CurrentScene = value; }

    public int SceneFirstKey { get => _SceneFirstKey; set => _SceneFirstKey = value; }

    private void Start()
    {

    }

    // ���� Scene�� ������ �ٲ�� �Ʒ��� �Լ� ������ �����ؾ� �Ѵ�.

    // 2���� �迭�� ��� Ű�� �����ϴ� �Լ�
    private int[,] SetEachFirstKey()
    {
        key = m_nFirstKey;
        for (int j = 0; j < sOd.userDataArr2.GetLength(1); j++)
        {
            sOd.userDataArr2[Row, j] = key;
            Debug.Log(sOd.userDataArr2[Row, j]);
            if (key == m_nLastKey)
            {
                break;
            }
            key++;
        }
        return sOd.userDataArr2;
    }
    // ���α׷� ���� �� ������ ���� Scene�� �ε�Ǹ� �� �ѹ��� �Ʒ��� �Լ���� �ʱ�ȭ�ȴ�
    public void InitialDataSetting()
    {
        float result = 0;
        Debug.Log("INIT");
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SetKey();
        key = m_nFirstKey;
        Debug.Log(key);
        while (key <= m_nLastKey)
        {
            m_skey = key.ToString();
            Debug.Log("INITIAL DATA SETTING" + key); JsonData.dataList.Add(m_skey, new PlayerJsonData("data_" + key, result));
            key++;
        }

    }
    public void SetSceneData(params object[] myVal)
    {
        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
        }
        keyLength = (m_nLastKey - m_nFirstKey) + 1;
        SetEachFirstKey();
        Debug.Log("keyLength = " + keyLength);
        Dictionary<string, object> mapName = new Dictionary<string, object>();

        for (int i = 0; i < keyLength; i++)
        {
            Debug.Log("ROW=" +Row);
            Debug.Log("USERDATA NULL = " + sOd.userDataArr2);
            int arg0 = sOd.userDataArr2[Row, i];
            string dataname = "data_" + arg0.ToString();
            Debug.Log("DATA" + i);
            Debug.Log("DATA"+i+"=" + dataname + "/" + myVal[i]);
            mapName.Add(key:
                dataname,
                value: myVal[i]);
            Debug.Log("DATA NAME + " + dataname);
            Debug.Log("data_" + arg0 + " = " + mapName[dataname]);
            Debug.Log(mapName["data_" + arg0]);
            object result = mapName[dataname];
            Debug.Log(result + "   " + arg0);
            JsonData.dataList[arg0.ToString()].Result = result;
        }
    }

    private int SetKey()//���⼭ ù��° Ű�� ������ Ű�� ����
    {
        int buildindex = SceneManager.GetActiveScene().buildIndex;
        switch (buildindex)
        {
            case 1: //bagpacking
                m_nFirstKey = 501;
                Row = 1;
                m_nLastKey = 511;
                break;
            case 2: //scoop
                m_nFirstKey = 601;
                Row = 2;
                m_nLastKey = 613;
                break;
            case 3: //Nummatch
                m_nFirstKey = 701;
                Row = 3;
                m_nLastKey = 717;
                break;
            case 4: //CRUM
                m_nFirstKey = 301;
                Row = 4;
                m_nLastKey = 313;
                break;
            case 5: //Schedule
                m_nFirstKey = 201;
                Row = 5;
                m_nLastKey = 213;
                break;
            case 6: //PlayPaddle 
                m_nFirstKey = 401;
                Row = 6;
                m_nLastKey = 420;
                break;
            case 7: //Conveyor
                m_nFirstKey = 801;
                Row = 7;
                m_nLastKey = 813;
                break;
            case 8:
                m_nFirstKey = 101;
                Row = 8;
                m_nLastKey = 104;
                break;

        }

        return m_nFirstKey;
    }
}

/*
[System.Serializable]
public class PlayerJsonData // List<T>�� �� Ŭ���� ����
{
    // PlayerData�� �ٷ� ����Ʈ�� �� �Ű������� ���� �͵��� �̸� ������ ������ �д�        
    public string DataName;
    public float Result;

    // ������ ������
    public PlayerJsonData(string dataName, float result)
    {
        this.DataName = dataName;
        this.Result = result;
    }
}

public class SetDataType : MonoBehaviour
{
    public PlayMakerFSM fsm;

    public int _SceneFirstKey;
    public int _CurrentScene;
    public int _Row;

    int key;
    int FirstKey;
    int eachLastKey;
    int eachFirstKey;
    int keyLength;

    // �������� �ε����� �ٲ� ���� �ٲ�� �ϴ� �͵� -> TotalFirstKey�� ���԰�, sOd�� ���� 2���� ��
    const int TotalFirstKey = 101;
    public JsonDataManager JsonData;
    DataSize sOd = new DataSize(7, 20);

    public int Row { get => _Row; set => _Row = value; }

    public int CurrentScene { get => _CurrentScene; set => _CurrentScene = value; }

    public int SceneFirstKey { get => _SceneFirstKey; set => _SceneFirstKey = value; }

    private void Start()
    {
        
    }
    public void SetFirstKeyInScenes(int scene)
    {
        // ���� �ε����� �ٲ�� ����ġ���� ���԰��� �ٲ��� �Ѵ�.
        switch (scene)
        {
            case 1: //bagpacking
                Row = 4;
                SceneFirstKey = 501;
                CurrentScene = scene;
                break;
            case 2: //scoop
                Row = 5;
                SceneFirstKey = 601;
                CurrentScene = scene;
                break;
             case 3: //Nummatch
                Row = 6;
                SceneFirstKey = 701;
                CurrentScene = scene;
                break;
            case 4: //CRUM
                Row = 2;
                SceneFirstKey = 301;
                CurrentScene = scene;
                break;
            case 5: //CRUM
                Row = 1;
                SceneFirstKey = 201;
                CurrentScene = scene;
                break;
            case 6: //PlayPaddle 
                Row = 3;
                SceneFirstKey = 401;
                CurrentScene = scene;
                break;
           
        }
    }

    // ���� Scene�� ������ �ٲ�� �Ʒ��� �Լ� ������ �����ؾ� �Ѵ�.
    private int SetFirstKey(int currentKey)
    {
        if (currentKey <= 199) eachFirstKey = 101;
        else if (currentKey <= 299) eachFirstKey = 201;
        else if (currentKey <= 399) eachFirstKey = 301;
        else if (currentKey <= 499) eachFirstKey = 401;
        else if (currentKey <= 599) eachFirstKey = 501;
        else if (currentKey <= 699) eachFirstKey = 601;
        else if (currentKey <= 799) eachFirstKey = 701;
        return eachFirstKey;
    }

    // �� ������ �����ϴ� �������� ���� �ٲ�� �Ʒ��� eachLastKey ������ �ش� ������ �°� �����ؾ� �Ѵ�.
    private int SetLastKey(int currentKey)
    {
        if (currentKey <= 199) eachLastKey = 107;
        else if (currentKey <= 299) eachLastKey = 213;
        else if (currentKey <= 399) eachLastKey = 311;
        else if (currentKey <= 499) eachLastKey = 420;
        else if (currentKey <= 599) eachLastKey = 511;
        else if (currentKey <= 699) eachLastKey = 612;
        else if (currentKey <= 799) eachLastKey = 713;
        return eachLastKey;
    }

    // 2���� �迭�� ��� Ű�� �����ϴ� �Լ�
    private int[,] SetEachFirstKey(int[,] arr2)
    {
        key = FirstKey;

        SetLastKey(key);
        for (int j = 0; j < sOd.userDataArr2.GetLength(1); j++)
            {
                sOd.userDataArr2[Row, j] = key;
                Debug.Log(sOd.userDataArr2[Row, j]);
                if (key == eachLastKey)
                {
                    break;
                }
                key++;
            }
        return sOd.userDataArr2;
    }

    private int GetKeyLength(int eachLastKey)
    {
        string temp_s = eachLastKey.ToString();
        temp_s = temp_s.Remove(0, 1);
        keyLength = int.Parse(temp_s);
        return keyLength;
    }
    public void ClearDataSetting()
    {
        JsonData.dataList.Clear();
    }

    // ���α׷� ���� �� ������ ���� Scene�� �ε�Ǹ� �� �ѹ��� �Ʒ��� �Լ���� �ʱ�ȭ�ȴ�
    public void InitialDataSetting()
    {
        float result = 0;
        Debug.Log("INIT");
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        SetFirstKeyInScenes(CurrentScene);
        FirstKey = SceneFirstKey;
        key = FirstKey;
        Debug.Log(SceneFirstKey);
        SetLastKey(key);
        while (key <= eachLastKey)
            {
                Debug.Log(key);
                JsonData.dataList.Add(key, new PlayerJsonData("data_" + key, result));
                key++;
            }

    }

    // PlayMaker�� ���� �����ϴ� Scene�� �Ʒ��� �Լ��� �����ε� �Ǿ� ���ȴ�
    public void SetSceneData()
    {
        CurrentScene = SceneManager.GetActiveScene().buildIndex;
        key = FirstKey;
        SetEachFirstKey(sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }

        SetLastKey(SceneFirstKey);
        GetKeyLength(eachLastKey);
        Debug.Log("keyLength = " + keyLength);

        //mapName ����
        Dictionary<string, FsmFloat> mapName = new Dictionary<string, FsmFloat>();

        for (int i = 0; i < keyLength; i++)
        {
            int arg0 = sOd.userDataArr2[Row, i];
            mapName.Add(key: string.Format("data_{0}", arg0),
                value: fsm.FsmVariables.GetFsmFloat("data_" + arg0));

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);

            JsonData.dataList[arg0].Result = mapName["data_" + arg0].Value;
        }
    }

    // C# Script�� ���� �����ϴ� Scene�� �Ʒ��� �Լ��� �����ε� �Ǿ� ���ȴ�
    public void SetSceneData(params float[] myVal)
    {
        key = FirstKey;
        SetEachFirstKey(sOd.userDataArr2);

        // value check test
        {
            Debug.Log("sceneIndex = " + CurrentScene.ToString());
            Debug.Log("sceneFirstKey = " + SceneFirstKey.ToString());
            Debug.Log("row = " + Row.ToString());
        }

        SetLastKey(SceneFirstKey);
        GetKeyLength(eachLastKey);
        Debug.Log("keyLength = " + keyLength);

        Dictionary<string, float> mapName = new Dictionary<string, float>();

        for (int i = 0; i < keyLength; i++)
        {
            int arg0 = sOd.userDataArr2[Row, i];
            mapName.Add(key: string.Format("data_{0}", arg0), value: myVal[i]);

            Debug.Log("data_" + arg0 + " = " + mapName[string.Format("data_" + arg0)]);
            Debug.Log(mapName["data_" + arg0]);
            Debug.Log(mapName.ContainsKey("data_" + arg0));
            float result = mapName["data_" + arg0];
            Debug.Log(result);
            JsonData.dataList[arg0].Result = result;
        }
    }

}
*/

