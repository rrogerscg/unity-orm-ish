using UnityEngine;
using UnityEngine.UI;

namespace ORMish
{
    [AddComponentMenu("ORMish/UI/Navigation Button")]
    [RequireComponent(typeof(Button))]

    public class NavigationButton : MonoBehaviour
    {
        [SerializeField]
        private ENavigationMenu _navTo;

        public void OnNavigationButtonUsed()
        {
            NavigationEvents.OnNavigateToMenu?.Invoke(_navTo);
        }
    }
}

