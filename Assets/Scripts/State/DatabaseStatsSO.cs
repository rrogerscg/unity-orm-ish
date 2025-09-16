using System.Collections.Generic;
using UnityEngine;


namespace ORMish
{
    [CreateAssetMenu(fileName = "DatabaseStats", menuName = "ORMish/DatabaseStats", order = -1)]
    public class DatabaseStatsSO : ScriptableObject
    {
        public List<string> TableNames = new();
    }
}