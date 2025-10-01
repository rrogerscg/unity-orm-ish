using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class CharacterSelectController : MonoBehaviour
    {
        [SerializeField] private CharacterCardTemplate _characterCardTemplate;
        [SerializeField] private GameObject _content;
        private List<CharacterCardTemplate> _characterCardTemplates;

        private void Awake()
        {
            _characterCardTemplates = new();
        }


        public void OnEnable()
        {
            Debug.Log("OnEnable CharacterSelectController");
            _characterCardTemplates.Clear();
            List<UserCharacter> userCharacters = PersistenceManager.Instance.UserCharacters;
            Debug.Log("User characters loaded for CharacterSelectController");
            foreach (UserCharacter character in userCharacters)
            {
                CharacterCardTemplate template = Instantiate(_characterCardTemplate, _content.transform);
                template.SetStats(character);
                template.gameObject.SetActive(true);
                _characterCardTemplates.Add(template);
            }
        }
    }

}
