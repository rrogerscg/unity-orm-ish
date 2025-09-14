using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ORMish
{
    public class SetMenuStateButton : MonoBehaviour
    {
        public CharacterCreationManager characterCreationManager;
        public CustomMenu menuGameObject;
        public Button button;
        public List<TransitionalObject> transitionalObjects;
        public RectTransform targetTransform;
        public Vector3 targetPosition;

        private void OnEnable()
        {
            button.onClick.AddListener(SetMenu);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(SetMenu);
        }
        protected virtual void SetMenu()
        {
            characterCreationManager.SetMenu(menuGameObject);
            foreach (TransitionalObject transitionalObject in transitionalObjects)
            {
                transitionalObject.Transition(targetTransform, targetPosition);
            }

        }
    }
}
