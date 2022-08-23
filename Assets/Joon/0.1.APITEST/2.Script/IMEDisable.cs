using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMEDisable : MonoBehaviour
{
    /*
     * ��й�ȣ �Է� �� �ѱ� �Է��� �������� ime�� disable �Ѵ�.
     * inputfield�� �̺�Ʈ �� �ϳ��� onValueChange�� IMFOFF�� ȣ���ϸ� �ѱ��Է��� ������
     * onValueChagne������ ȣ���� �� ù��° character�� �ѱ۷� �ԷµǴµ� �̸� ��������
     * event trigger ������Ʈ�� �߰��� select �� �� ime�� disable �ϰ� �صξ���.
     * select�Ҷ��� IMEOFF�� ȣ���ҽ� �۵����� �ʴ´�
     * ���� ��ũ : https://run-a-way.tistory.com/m/70
     * */
    public void IMEOFF() { Input.imeCompositionMode = IMECompositionMode.Off; Debug.Log("OFF");  }
    public void IMEON() { Debug.Log("ON"); Input.imeCompositionMode = IMECompositionMode.Auto; }
}
