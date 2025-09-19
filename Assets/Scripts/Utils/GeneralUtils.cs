using System;
using System.Collections.Generic;

namespace Example
{
    public static class GeneralUtils
    {
        private static readonly Random _random = new Random();

        public static void Shuffle<T>(ref List<T> list)
        {   
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int randomIndex = UnityEngine.Random.Range(i, list.Count - 1);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static T GetRandomEnum<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(_random.Next(values.Length));
        }
    }
}
