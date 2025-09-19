using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Example
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Colors", order = 1)]
    public class SO_Colors : ScriptableObject
    {

        public List<ColorHash> colors;
        public Button colorButtonPrefab;
    }
}
