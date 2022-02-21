using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;//for Outlinable
using BNG;
namespace CleanUp {
public class Arrange : MonoBehaviour {

    public enum ARRANGE  { 
        BOX, LAMP, GAME_PAD, GLOBE,  MOUTH_WASH, TOY_HAMMER,  TOY_APPLE, TOY_CAR1, TOY_CAR2, TOY_GUN, 
        TOY_BALL, TOY_BUNNY, BOOK1, BOOK2, BOOK3,  PANTS1 , PANTS2 , PANTS3 , NONE  
    }        
    
    //최종적으로 각 물건에 대한 평가결과 저장
    public struct ARRANGE_INFO   {
        public ARRANGE eArrange;         //종류
        public string  Object_name;      //나에게 와야할 GameObject Name                   
        public bool    bCleanable;       //청소대상여부
        public bool    bPositioned;      //제위치여부
        public int     nGrabCount;       //Player가 내 물건을 잡은 횟수
        public float   fGrabTime;        //Player가 내 물건을 잡은 시간
        public int     nPosiCount;       //자기위치에 둔 횟수
        public string  kor_name;         //한글명
                                         
        public ARRANGE_INFO(ARRANGE eArrange, string Object_name, bool bCleanable , bool bPositioned, 
            int  nGrabCount , float fGrabTime , int  nPosiCount,string kor_name )  {         
            this.eArrange        =  eArrange;                              
            this.Object_name     =  Object_name;                        
            this.bCleanable      =  bCleanable ; 
            this.bPositioned     =  bPositioned;
            this.nGrabCount      =  nGrabCount ; 
            this.fGrabTime       =  fGrabTime  ;  
            this.nPosiCount      =  nPosiCount ; 
            this.kor_name        =  kor_name;
        }
    }
   
    public static ARRANGE_INFO[] CDB = new ARRANGE_INFO[]  {        
		new ARRANGE_INFO(ARRANGE.BOX,         "Box",       false, false, 0, 0.0f, 0, "박스"   ),             
        new ARRANGE_INFO(ARRANGE.LAMP,        "Lamp",      false, false, 0, 0.0f, 0, "램프"   ),                     
        new ARRANGE_INFO(ARRANGE.GAME_PAD,    "GamePad",   false, false, 0, 0.0f, 0, "게임패드" ),    
        new ARRANGE_INFO(ARRANGE.GLOBE,       "Globe",     false, false, 0, 0.0f, 0, "지구본"  ),             
        new ARRANGE_INFO(ARRANGE.MOUTH_WASH,  "MouthWash", false, false, 0, 0.0f, 0, "로션"   ),             
  		new ARRANGE_INFO(ARRANGE.TOY_HAMMER,  "ToyHammer", false, false, 0, 0.0f, 0, "뽕망치"  ),                     
        new ARRANGE_INFO(ARRANGE.TOY_APPLE,   "ToyApple",  false, false, 0, 0.0f, 0, "사과"   ),             
        new ARRANGE_INFO(ARRANGE.TOY_CAR1,    "ToyCar1",   false, false, 0, 0.0f, 0, "자동차1" ),                     
        new ARRANGE_INFO(ARRANGE.TOY_CAR2,    "ToyCar2",   false, false, 0, 0.0f, 0, "자동차2" ),                     
        new ARRANGE_INFO(ARRANGE.TOY_GUN,     "ToyGun",    false, false, 0, 0.0f, 0, "총"     ),                     
        new ARRANGE_INFO(ARRANGE.TOY_BALL,    "ToyBall",   false, false, 0, 0.0f, 0, "공"     ),                     
        new ARRANGE_INFO(ARRANGE.TOY_BUNNY,   "ToyBunny",  false, false, 0, 0.0f, 0, "토끼"   ),                     
        new ARRANGE_INFO(ARRANGE.BOOK1,       "Book1",     false, false, 0, 0.0f, 0, "책1"    ),             
        new ARRANGE_INFO(ARRANGE.BOOK2,       "Book2",     false, false, 0, 0.0f, 0, "책2"    ),             
        new ARRANGE_INFO(ARRANGE.BOOK3,       "Book3",     false, false, 0, 0.0f, 0, "책3"    ),                     
        new ARRANGE_INFO(ARRANGE.PANTS1,      "Pants1",    false, false, 0, 0.0f, 0, "바지1"  ),             
        new ARRANGE_INFO(ARRANGE.PANTS2,      "Pants2",    false, false, 0, 0.0f, 0, "바지2"  ),             
        new ARRANGE_INFO(ARRANGE.PANTS3,      "Pants3",    false, false, 0, 0.0f, 0, "바지3"  ),             
    };    
   
    /**************************************************************************/
    /* Member Variable
    /**************************************************************************/  
    // Public... Assigned in Editor
    public ARRANGE    m_eArrange       = ARRANGE.BOX; //Editor에서 Kind를 할당해 줍니다
    public bool       m_bCleanable     = false;       //청소할 대상인지여부
    public bool       m_bPositioned    = false;       //제자리에 위치여부    
    public bool       m_bDroppable     = false;       //Positon뒤에 Drop 여부
    public bool       m_bNearCatch     = false;       //갖다놓을 장소 근처에 오면 인식여부
    public float      m_fAccept_radius = 0.2f;        //콜라이더외에 반경안에 들어오면 갖다 놓은것으로 간주
    public bool       m_bGrabbed;                     //현재 잡혔는지
    //Private
    Rigidbody         m_RigidBody;
    
    //평가자료
    int               v_nGrabCount;                   // Player가 내 물건을 잡은 횟수
    float             v_fGrabTime;                    // Player가 내 물건을 잡은 시간
    int               v_nPosiCount;                   // 자기위치에 둔 횟수
    /**************************************************************************
    // Method Start
    ***************************************************************************/
    //Grabbed 되었을 때 Action을 정의합니다 called by Guide
    public void OnGrab() {        
        m_bGrabbed   = true;
        SetRigidBody(true);     //droppable 물건은 Rigidbocy가 disable되어 있으므로 손으로 잡으면 Rigidbody 활성화
        m_Target.gameObject.SetActive(true);  //갖다놓을 녹색 Target을 보여주고
        v_nGrabCount ++; // Eval
    }
    //Release 되었을 때 Action을 정의하합니다 called by Guide
    public void OnGrabRelease() {
        m_bGrabbed = false;
        if (m_Target.gameObject.activeSelf) m_Target.DeActivate();
    }

    void DropAt(Transform tf){
        transform.position      = tf.position;
        transform.eulerAngles   = tf.eulerAngles;
    }
    
    public void SetRigidBody(bool bOn){
        m_RigidBody.mass        = (bOn ? oMass        : 0);
        m_RigidBody.drag        = (bOn ? oDrag        : 0);
        m_RigidBody.angularDrag = (bOn ? oAngularDrag : 0); 
        m_RigidBody.useGravity  = (bOn ? oUseGravity  : false); 
        m_RigidBody.constraints = (bOn ? oConstraints : RigidbodyConstraints.FreezeAll);
    }

    // 내위치에 갖다놈
    // 원래 정리대상이 아닌데 다시 갖다 놓는경우, 정리대상을 갖다 놓는경우 구분할것
    void SetPositoned()  { 
        m_bPositioned = true;    //CDB갱신
        //내위치를 Target의 위치에 둡니다
        if(!m_bDroppable) SetRigidBody(false); 
        DropAt(m_Target.transform);
        StartCoroutine("AdhereTarget");
        Outlinable outline = GetComponent<Outlinable>();    //Ouline을 가져와서
        StartCoroutine(OulineBlinkOut(outline));            //흰색을 점멸후 제거      
         //정리할 물건일 경우 정리된 것으로 바꾸고 파티클 전시 
        if(m_bCleanable) {     
            m_Guide.SetArranged(m_eArrange, m_Target.transform);  //정리한것을 Guide에게 알려줌  
        }
        v_nPosiCount++; // Eval
    }
    // 정리할곳에서 떨어짐 valid for only "not droppable"
    void Detach() {
        m_bPositioned = false;    //CDB갱신
    }
    // 잡고있으면 계속 위치가 바뀌므로 놓을때까지 기다렸다가 다시 Target위치에 둠
    IEnumerator AdhereTarget()  {
        while(Guide.m_eGrabbedArrange != ARRANGE.NONE) yield return new WaitForSeconds(0.1f);
        DropAt(m_Target.transform);
    }

    //정리했을때 Outline 흰색을 점멸후 제거
    IEnumerator OulineBlinkOut(Outlinable outline) {
        if(!outline) yield break;
        outline.OutlineParameters.Color = HUD.COLOR_OUTLINE_WHITE; //흰색으로 변경
        yield return new WaitForSeconds(1f);
        for(int i=0; i < 6; i++) {
            outline.enabled = !outline.enabled;
            yield return new WaitForSeconds(0.3f);
        }
        outline.enabled = false;
    }

    /**************************************************************************
    // Monobehavier Start
    ***************************************************************************/
    Guide            m_Guide;   
    Target           m_Target;
    
    // Start is called before the first frame update
    //Original RigidBody Properties Backup
    float oMass, oDrag, oAngularDrag;  bool  oUseGravity ;  RigidbodyConstraints oConstraints;

    void Start() { 
        m_Guide       = GameObject.Find("Guide").GetComponent<Guide>();
        m_Target      = GameObject.Find("/Objects/Targets/"+CDB[(int)m_eArrange].Object_name).GetComponent<Target>();
        m_RigidBody   = GetComponent<Rigidbody>();
        oMass         = m_RigidBody.mass;
        oDrag         = m_RigidBody.drag;
        oAngularDrag  = m_RigidBody.angularDrag; 
        oUseGravity   = m_RigidBody.useGravity; 
        oConstraints  = m_RigidBody.constraints;
        
        //원래 제자리에 있는 물건이면 위치 고정
        if(m_bPositioned) {
            DropAt(m_Target.transform);
            if(!m_bDroppable) SetRigidBody(false); 
        }
    }

    // Update is called once per frame        
    void Update() {        
        if(m_bGrabbed) v_fGrabTime += Time.deltaTime; //EVAL
        //가져다놓을 위치와 거리를 계산해 둡니다.
        float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        if(distance < m_fAccept_radius) { //반경안에 있고
            if (!m_bPositioned && m_bNearCatch)   SetPositoned(); //아직정리가 안된상태이면서 NearCatch가 설정되어 있으면 정리처리
        } else{ //반경밖에 있고
            if (!m_bDroppable && m_bPositioned)   Detach(); //정리된상태에서 Droppable이 아닌것이 멀어지면 Detach처리
        }
    }
    
    // 트리거(Target)와 접촉되면 갖다놓은것으로 처리합니다.
    void OnTriggerEnter(Collider other) {        
        //정리안된상태에서 TriggerEnter는 Attach
        if(!m_bPositioned && other.gameObject.name == gameObject.name) {                                    
            SetPositoned();//정리한 것으로 처리
        }
    }    

     //평가종료시 Guide가 호출
    public void SaveResult() {
        CDB[(int)m_eArrange].bCleanable  =  m_bCleanable;
        CDB[(int)m_eArrange].bPositioned =  m_bPositioned;   
        CDB[(int)m_eArrange].nGrabCount  =  v_nGrabCount; 
        CDB[(int)m_eArrange].fGrabTime   =  v_fGrabTime;  
        CDB[(int)m_eArrange].nPosiCount  =  v_nPosiCount; 
    }
}
}
