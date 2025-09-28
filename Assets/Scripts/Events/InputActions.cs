using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Example
{
    public static class InputActions
    {
        private static ExampleInputActions _inputs;
        public static void Initialize()
        {
            if (_inputs == null)
            {
                _inputs = new ExampleInputActions();
                _inputs.Enable();
                _inputs.Dev.Enable();
            }
            else
            {
                Debug.LogWarning("InputActionsMap already initialized... will not initialize again, so stop asking. ..|..");
            }
        }

        // Add method to subscribe to events
        public static void SubscribeToToggleConsole(Action<InputAction.CallbackContext> callback)
        {
            if (_inputs != null)
            {
                _inputs.Dev.ToggleConsole.performed += callback;
            }
        }

        public static void UnsubscribeFromToggleConsole(Action<InputAction.CallbackContext> callback)
        {
            if (_inputs != null)
            {
                _inputs.Dev.ToggleConsole.performed -= callback;
            }
        }

        // Add method to subscribe to events
        public static void SubscribeToExecuteCommand(Action<InputAction.CallbackContext> callback)
        {
            if (_inputs != null)
            {
                _inputs.Dev.ExecuteCommand.performed += callback;
            }
        }

        public static void UnsubscribeFromExecuteCommand(Action<InputAction.CallbackContext> callback)
        {
            if (_inputs != null)
            {
                _inputs.Dev.ExecuteCommand.performed -= callback;
            }
        }
    }
}
