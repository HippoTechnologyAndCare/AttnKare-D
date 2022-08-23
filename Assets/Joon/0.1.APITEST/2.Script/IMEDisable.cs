using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMEDisable : MonoBehaviour
{
    /*
     * 비밀번호 입력 시 한글 입력을 막기위해 ime를 disable 한다.
     * inputfield의 이벤트 중 하나인 onValueChange에 IMFOFF를 호출하면 한글입력이 막힌다
     * onValueChagne에서만 호출할 시 첫번째 character는 한글로 입력되는데 이를 막기위해
     * event trigger 컴포넌트를 추가해 select 할 시 ime를 disable 하게 해두었다.
     * select할때만 IMEOFF를 호출할시 작동하지 않는다
     * 참고 링크 : https://run-a-way.tistory.com/m/70
     * */
    public void IMEOFF() { Input.imeCompositionMode = IMECompositionMode.Off; Debug.Log("OFF");  }
    public void IMEON() { Debug.Log("ON"); Input.imeCompositionMode = IMECompositionMode.Auto; }
}
