using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    [AddComponentMenu("ORMish/UI/Navigation Button")]
    [RequireComponent(typeof(Button))]

    public class NavigationButton : MonoBehaviour
    {
        [SerializeField]
        private ENavigationMenu _navTo;

        private void OnEnable()
        {
            PersistenceEvents.UserCharactersExist += UserCharactersExist;
        }

        private void OnDisable()
        {
            PersistenceEvents.UserCharactersExist -= UserCharactersExist;
        }

        public void OnNavigationButtonUsed()
        {
            NavigationEvents.OnNavigateToMenu?.Invoke(_navTo);
        }

        private void UserCharactersExist(bool exists)
        {
            if(!exists && _navTo == ENavigationMenu.SelectCharacter)
            {
                GetComponent<Button>().interactable = false;
            } else
            {
                GetComponent<Button>().interactable = true;
            }
        }
    }
}

