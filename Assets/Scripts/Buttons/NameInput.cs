using TMPro;
using UnityEngine;


namespace Example
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
