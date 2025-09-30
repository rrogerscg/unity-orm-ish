using Example;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Example
{
    public class CharacterCardTemplate : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _id;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _hairColor;
        [SerializeField] private TMP_Text _eyeColor;
        [SerializeField] private TMP_Text _skinColor;


        public void SetStats(UserCharacter uc)
        {
            _id.SetText($"{uc.Id}");
            _name.SetText(uc.Name);
            _hairColor.SetText(uc.HairColor);
            _eyeColor.SetText(uc.EyeColor);
            _skinColor.SetText(uc.SkinColor);
        }
    }

}

