using EPOOutline;
using HutongGames.PlayMaker;
using UnityEngine;

namespace BNG
{
    /// <summary>
    /// A simple laser pointer that draws a line with a dot at the end
    /// </summary>
    public class LaserPointer_V3 : MonoBehaviour
    {
        public float MaxRange = 2.5f;
        public LayerMask ValidLayers;
        public LineRenderer line1;
        public LineRenderer line2;
        public GameObject cursor;
        public GameObject _cursor;
        public GameObject LaserEnd;
        public GameObject hitObject;

        public bool Active = true;

        public PlayMakerFSM GoFsm;
        public Grabber graber;
        
        private int lineEndPosition;
        private FsmObject fsmObject;
        private FsmGameObject fsmG_Obj;

        /// <summary>
        /// 0.5 = Line Goes Half Way. 1 = Line reaches end.
        /// </summary>
        [UnityEngine.Tooltip("Example : 0.5 = Line Goes Half Way. 1 = Line reaches end.")]
        public float LineDistanceModifier = 0.8f;


        //reference로 지정된 object에 있는 FSM으로 Event를 보낸다.
        private void SendEvent(string currentEvent)
        {
            fsmG_Obj = GoFsm.FsmVariables.GetFsmGameObject("grab_g");
            GoFsm.SendEvent(currentEvent);
        }

        private void Awake()
        {           
            GoFsm.gameObject.GetComponent<PlayMakerFSM>();

            if (cursor)
            {
                _cursor = GameObject.Instantiate(cursor);
            }

            // If no Line Renderer was specified in the editor, check this Transform
            if (line1 == null)
            {
                line1 = GetComponent<LineRenderer>();
            }
            if (line2 == null)
            {
                line2 = GetComponent<LineRenderer>();
            }

            // Line Renderer is positioned using world space
            if (line1 != null)
            {
                line1.useWorldSpace = true;
            }
        }

        private void LateUpdate()
        {
            if (Active)
            {
                line1.enabled = true;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
                {
                    // Add dot at line's end
                    LaserEnd.transform.position = hit.point;
                    LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    if (hit.transform.gameObject.tag == "Necessary" || hit.transform.gameObject.tag == "Necessary_Book" || hit.transform.gameObject.tag == "Necessary_Pencil")
                    {

                        hitObject = hit.collider.gameObject;                                                                      
                        if (InputBridge.Instance.RightTriggerDown == true)
                        {                                                        
                            if (hitObject.GetComponent<Outlinable>().enabled != true)
                            {
                                hitObject.GetComponent<Outlinable>().enabled = true;

                                SendEvent("Enter");
                                fsmG_Obj.Value = hitObject;
                            }                            
                        }                            
                    }
                }

                // Set position of the cursor
                if (_cursor != null)
                {
                    if (line1)
                    {
                        if (hit.distance > 0)
                        {
                            line1.enabled = true;
                            line2.enabled = false;
                            line1.useWorldSpace = false;
                            line1.SetPosition(0, Vector3.zero);
                            line1.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, hit.point) * LineDistanceModifier));
                            Debug.Log("hit coll = " + hit.collider.name);                            
                            //hitObject = hit.collider.gameObject;                            
                        }

                        else if (hit.distance <= 0)
                        {
                            line1.enabled = false;
                            line2.enabled = true;
                            line2.SetPosition(0, Vector3.zero);
                            line2.SetPosition(1, new Vector3(0, 0, MaxRange));
                            LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                            LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);                                                        
                        }
                    }
                    else
                    {
                        line1.useWorldSpace = false;
                        line1.SetPosition(0, Vector3.zero);
                        line1.SetPosition(1, new Vector3(0, 0, MaxRange));
                        line1.enabled = hit.distance > 0;
                        LaserEnd.transform.position = new Vector3(0, 0, MaxRange);
                        LaserEnd.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    }
                }
            }
            else
            {
                LaserEnd.gameObject.SetActive(false);

                if (line1)
                {
                    line1.enabled = false;
                }
                else if (line2)
                {
                    line2.enabled = false;
                }
            }
        }
    }
}