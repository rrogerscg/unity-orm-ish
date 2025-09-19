using System.Collections.Generic;
using UnityEngine;


namespace Example
{
    public class CharacterCreationManager : MonoBehaviour
    {
        public GameObject body;
        public GameObject eyes;
        public GameObject hair;
        public CustomMenu initialMenu;
        private SpriteRenderer bodyImage;
        private SpriteRenderer eyeImage;
        private SpriteRenderer hairImage;
        public List<CustomMenu> menuGameObjects;

        private void Start()
        {
            bodyImage = body.GetComponent<SpriteRenderer>();
            eyeImage = eyes.GetComponent<SpriteRenderer>();
            hairImage = hair.GetComponent<SpriteRenderer>();
            SetMenu(initialMenu);
        }

        public void SetBodyColor(Color color)
        {
            bodyImage.color = color;
        }

        public void SetEyeColor(Color color)
        {
            eyeImage.color = color;
        }

        public void SetHairColor(Color color)
        {
            hairImage.color = color;
        }

        public void SetMenu(CustomMenu menu)
        {
            foreach (CustomMenu menuObject in menuGameObjects)
            {
                if (menuObject == menu)
                {
                    menuObject.Activate();
                }
                else
                {
                    menuObject.Deactivate();
                }

            }

        }

    }
}
