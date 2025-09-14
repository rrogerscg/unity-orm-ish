using System.Collections;
using UnityEngine;

namespace ORMish
{
    // This object will transition to have a new parent
    public class TransitionalObject : MonoBehaviour
    {
        private float speed = 2.0f;
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Transition(RectTransform newParent, Vector3 targetPosition)
        {
            rectTransform.SetParent(newParent);
            StartCoroutine(AnimatePosition(targetPosition));
        }

        private IEnumerator AnimatePosition(Vector3 targetPosition)
        {
            Vector3 startPosition = rectTransform.anchoredPosition;
            float elapsedTime = 0.0f;

            while (elapsedTime < 1.0f)
            {
                elapsedTime += Time.deltaTime * speed;
                float t = Mathf.SmoothStep(0.0f, 1.0f, elapsedTime);
                Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);
                rectTransform.anchoredPosition = newPosition;
                yield return null;
            }
        }
    }
}
