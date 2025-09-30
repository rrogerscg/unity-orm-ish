using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class CharacterSelectController : MonoBehaviour
    {
        [SerializeField] private CharacterCardTemplate _characterCardTemplate;
        [SerializeField] private GameObject _content;
        private List<CharacterCardTemplate> _characterSelectControllers;

        private void Start()
        {
            _characterSelectControllers = new();
        }


        public void OnEnable()
        {
            _characterSelectControllers.Clear();
            PersistenceManager.Instance.LoadUserCharacters();
            List<UserCharacter> userCharacters = PersistenceManager.Instance.UserCharacters;
            foreach (UserCharacter character in userCharacters)
            {
                CharacterCardTemplate template = Instantiate(_characterCardTemplate, _content.transform);
                template.SetStats(character);
                _characterSelectControllers.Add(template);
            }
        }
    }

}
