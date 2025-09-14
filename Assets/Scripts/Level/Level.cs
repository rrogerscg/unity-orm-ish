using UnityEngine;


namespace ORMish
{
    public abstract class Level : MonoBehaviour
    {
        public static Level ActiveLevel { get; set; }

        protected virtual void Start()
        {
            Debug.Log("Start called for " + this.GetType());
        }
    }
}
