using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tutorial {
    public class InsertPassword : MonoBehaviour
    {
        public bool DebugMode= false;
        private bool m_debug = true;
        Guide_Tutorial Manager;
        [SerializeField] private TextMeshProUGUI txt_Password;
        private List<int> m_listCorrect = new List<int>(){4,7,1,3,9,6};
        private List<int> m_listDebug = new List<int>() { 1,4,7,8,9,6 };
        public string m_strCorrect;
        private List<int> m_listAnswer = new List<int>();
        private List<bool> m_listCompare = new List<bool>();
        [SerializeField] private int m_nCorrectIndex;
        [SerializeField] private int m_nCorrectNumber;
        [SerializeField] private int m_nCorrectOrder;
        [SerializeField] private int m_listCount;
        public object[] arrData;
        private void Start()
        {
            Manager = GetComponent<Guide_Tutorial>();
            foreach (int num in m_listCorrect) { m_strCorrect += num.ToString(); }
        }
        private void Update()
        {
            if (DebugMode && m_debug) { Done(); m_debug = false; }
        }
        public string PasswordStart()
        {
            for(int i = 0; i< 6; i++) { int random = Random.Range(0, 9); m_listCorrect.Add(random); }
            foreach (int num in m_listCorrect) { m_strCorrect += num.ToString(); }
            return m_strCorrect;
            
        }
        public void Press(PasswordType type)
        {
            switch (type)
            {
                case (PasswordType.One): InsertNumber(1);  break;
                case (PasswordType.Two): InsertNumber(2); break;
                case (PasswordType.Three): InsertNumber(3); break;
                case (PasswordType.Four): InsertNumber(4); break;
                case (PasswordType.Five): InsertNumber(5); break;
                case (PasswordType.Six): InsertNumber(6); break;
                case (PasswordType.Seven): InsertNumber(7); break;
                case (PasswordType.Eight): InsertNumber(8); break;
                case (PasswordType.Nine): InsertNumber(9); break;
                case (PasswordType.Zero): InsertNumber(0); break;
                case (PasswordType.Del): Delete(); break;
                case (PasswordType.Confirm): Confirm(); break;
            }
        }
        void Delete()
        {
            if(m_listAnswer.Count > 0)
            {
                txt_Password.text = txt_Password.text.Substring(0, txt_Password.text.Length - 1);
                m_listAnswer.RemoveAt(m_listAnswer.Count - 1);
            }

        }
        void Done()
        {
            m_listAnswer = m_listDebug;
            Confirm();
        }
        void Confirm()
        {
           // if (m_listAnswer.Count < 6) return;
            int m_index1, m_index2 =0;
            int m_index3 =0;
            foreach (int num in m_listAnswer){
                if (m_listCorrect.Contains(num)) {
                    m_index1=m_index2 = 0;
                    m_nCorrectNumber++;
                    if (num == m_listCorrect[m_listAnswer.IndexOf(num)]) m_nCorrectIndex++;
                    m_index1 = m_listCorrect.IndexOf(num);
                    m_index2 = m_listAnswer.IndexOf(num);
                    while(m_listAnswer[m_index2] == m_listCorrect[m_index1] && m_index3 < m_index2 ) {
                        Debug.Log("CURRENT Index +" + m_index2 + "/" + m_index1); m_nCorrectOrder++; m_index2++; m_index1++; 
                        if (m_index2 >= 5 || m_index1 >= 5) break; }
                }
                m_index3 = m_index2;
            }
            m_listCount = m_listAnswer.Count;
            arrData = new object[] { m_nCorrectIndex, m_nCorrectOrder, m_nCorrectNumber, m_listCount };
           StartCoroutine(Manager.PasswordEntered());
        }


        void InsertNumber(int num)
        {
            if (m_listAnswer.Count < 6) { m_listAnswer.Add(num); txt_Password.text += num; }
        }

    }
}

