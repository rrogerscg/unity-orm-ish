using UnityEngine;


namespace Example
{
    [System.Serializable]
    public class ColorHash
    {
        public Color colorValue;
        public string colorName;

        public ColorHash(Color color, string name)
        {
            colorValue = color;
            colorName = name;
        }
    }
}