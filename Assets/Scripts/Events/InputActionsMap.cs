using UnityEngine.InputSystem;
using UnityEngine;


namespace Example
{
    public static class InputActionsMap
    {
        private static ExampleInputActions _inputs;
        public static void Initialize()
        {
            if (_inputs == null)
            {
                _inputs = new ExampleInputActions();
                _inputs.Enable();
                _inputs.Dev.Enable();
                _inputs.Dev.ToggleConsole.performed += InputActionEvents.ToggleConsole;
                _inputs.Dev.ExecuteCommand.performed += InputActionEvents.OnExecuteCommand;

            }
            else
            {
                Debug.LogWarning("InputActionsMap already initialized... will not initialize again, so stop asking. ..|..");
            }
        }
    }
}
