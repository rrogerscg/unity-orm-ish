using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private Queue<ToastTemplate> _toastPool;
        private Queue<ToastTemplate> _activeToastQueue;
        private Dictionary<ToastTemplate, Coroutine> _toastRoutineByTemplate;

        [SerializeField]
        private VerticalLayoutGroup _layoutGroup;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            _toastPool = new(_queueCapacity);
            _activeToastQueue = new(_queueCapacity);
            _toastRoutineByTemplate = new();
        }

        private void Start()
        {
            for (int i = 0; i < _queueCapacity; i++)
            {
                ToastTemplate template = Instantiate(_templateObject, _layoutGroup.transform);
                _toastPool.Enqueue(template);
            }
        }

        private void OnEnable()
        {
  
            ToastEvents.OnShowEvent += ShowToast;
        }

        private void ShowToast(string message)
        {
            ToastTemplate toast;

            if (_toastPool.Count > 0)
            {
                toast = _toastPool.Dequeue();
            }
            else
            {
                toast = _activeToastQueue.Dequeue();

                // initially, I tried using IEnumerator directly, however it turns out that is not reliable for
                // targeting the correct Coroutine in Unity.  Instead, we cache the actual Coroutine in a dict object
                if (_toastRoutineByTemplate.TryGetValue(toast, out Coroutine oldCoroutine))
                {
                    StopCoroutine(oldCoroutine);
                    _toastRoutineByTemplate.Remove(toast);
                }

                toast.gameObject.SetActive(false);
            }

            _activeToastQueue.Enqueue(toast);
            Coroutine newCoroutine = StartCoroutine(ShowToastRoutine(toast, message));
            _toastRoutineByTemplate[toast] = newCoroutine;
        }

        private IEnumerator ShowToastRoutine(ToastTemplate toast, string message)
        {
            toast.SetText(message);
            toast.gameObject.SetActive(true);
            yield return new WaitForSeconds(20f);
            toast.gameObject.SetActive(false);

            _toastRoutineByTemplate.Remove(toast);
            _activeToastQueue = new Queue<ToastTemplate>(_activeToastQueue.Where(t => t != toast));
            _toastPool.Enqueue(toast);
        }
    }
}
