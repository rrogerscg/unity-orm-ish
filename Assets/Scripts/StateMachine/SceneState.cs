using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ORMish.UnlockableScriptableObject;

namespace ORMish
{
    public class SceneState : MonoBehaviour, IState
    {
        public enum EGameNames
        {
            None,
            CharacterCreation
        }
        [SerializeField]
        public EGameNames GameName;

        [SerializeField]
        public ETheme Theme;

        public enum ESceneType
        {
            Game,
            LevelSelect,
            ThemeSelection
        }


        [SerializeField]
        public ESceneType SceneType;

        private string _name;
        public string Name => _name;

        public void Enter()
        {
            Debug.Log("[SceneState::Enter] " + Name);
            GameManager.Instance.ActiveGameName = GameName;
            switch (SceneType)
            {
                case ESceneType.Game:
                    _name = GameName.ToString();
                    break;
                case ESceneType.LevelSelect:
                    _name = ESceneType.LevelSelect.ToString();
                    break;
                case ESceneType.ThemeSelection:
                    _name = ESceneType.ThemeSelection.ToString();
                    GameManager.Instance.LoadThemesByActiveGameName();
                    break;
            }
            StartCoroutine(LoadScene());
        }

        public void Exit()
        {
            Debug.Log("[SceneState::Exit] " + Name);
        }

        // Coroutine to load the scene with transition animation
        private IEnumerator LoadScene()
        {
            // Play transition animation (e.g., fade out)
            yield return StartCoroutine(PlayTransitionAnimation());
            SceneManager.LoadScene(_name);
        }

        // Placeholder for transition animation coroutine
        private IEnumerator PlayTransitionAnimation()
        {
            // Implement your transition animation here
            yield return new WaitForSeconds(1.0f); // Example delay
        }
    }
}
