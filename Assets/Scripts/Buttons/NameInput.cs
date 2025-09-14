using TMPro;
using UnityEngine;


namespace ORMish
{
    public class NameInput : MonoBehaviour
    {
        public TMP_InputField characterName;

        public string GetValue()
        {
            return characterName.text;
        }
    }
}
