using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial {
    public enum PasswordType { One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Zero, Del, Confirm}
    public class Password : MonoBehaviour
    {
        public PasswordType _passwordtype { get; set; }
        public PasswordType enum_Type;
        public InsertPassword PW_Insert;

        public void OnClick()
        {
            PW_Insert.Press(enum_Type);
        }

    }
}

