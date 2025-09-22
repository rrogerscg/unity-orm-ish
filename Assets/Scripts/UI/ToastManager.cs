using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ToastManager : MonoBehaviour
    {
        public static class ToastEvents
        {
            public static Action<string> OnShowEvent;
        }

        [SerializeField]
        private ToastTemplate _templateObject;
        private readonly int _queueCapacity = 6;
        private Queue<ToastTemplate> _toastQueue;
        [SerializeField]
        private VerticalLayoutGroup _layoutGroup;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            _toastQueue = new Queue<ToastTemplate>(_queueCapacity);
        }

        private void Start()
        {
            for (int i = 0; i < _queueCapacity; i++)
            {
                ToastTemplate template = Instantiate(_templateObject, _layoutGroup.transform);
                _toastQueue.Enqueue(template);
            }
        }

        private void OnEnable()
        {
            ToastEvents.OnShowEvent += ShowToast;
        }

        private void ShowToast(string message)
        {
            StartCoroutine(ShowToastRoutine(message));
        }

        private IEnumerator ShowToastRoutine(string message)
        {
            ToastTemplate toast = _toastQueue.Dequeue();
            toast.SetText(message);
            toast.gameObject.SetActive(true);

            yield return new WaitForSeconds(6f);

            toast.gameObject.SetActive(false);
            _toastQueue.Enqueue(toast);
        }
    }
}
