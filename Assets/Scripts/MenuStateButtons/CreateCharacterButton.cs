using UnityEngine;
using UnityEngine.UI;


namespace ORMish
{
    public class CreateCharacterButton : MonoBehaviour
    {
        private SkinColorManager skinColorManager;
        private HairColorManager hairColorManager;
        private EyeColorManager eyeColorManager;
        private NameInput nameInput;
        private Button completeButton;
        [SerializeField]
        private SceneState nextScene;

        private void Start()
        {
            skinColorManager = FindFirstObjectByType<SkinColorManager>();
            hairColorManager = FindFirstObjectByType<HairColorManager>();
            eyeColorManager = FindFirstObjectByType<EyeColorManager>();
            nameInput = FindFirstObjectByType<NameInput>();

        }

        private void OnEnable()
        {
            completeButton = gameObject.GetComponent<Button>();
            completeButton.onClick.AddListener(CreateCharacter);
        }

        private void OnDisable()
        {
            completeButton.onClick.RemoveListener(CreateCharacter);
        }

        private void CreateCharacter()
        {
            try
            {

                UserCharacter newUser = new(nameInput.GetValue(),
                                   skinColorManager.getSelectedColorHash().colorName,
                                   hairColorManager.getSelectedColorHash().colorName,
                                   eyeColorManager.getSelectedColorHash().colorName);

                newUser.SetAsActiveUserCharacter();
                newUser.Put();
                UserCharacter.SaveTable();
                SceneStateManager.Instance.ChangeState(nextScene);
            }
            catch (UniqueUserCharacterNameError e)
            {
                // Tell user that name is not unique
                Debug.LogError(e);
            }
        }
    }
}
