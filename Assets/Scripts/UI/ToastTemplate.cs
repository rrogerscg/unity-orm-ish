using TMPro;
using UnityEngine;

namespace Example
{
    public class ToastTemplate : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}

