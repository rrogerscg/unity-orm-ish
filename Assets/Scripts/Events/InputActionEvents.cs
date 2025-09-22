using System;
using UnityEngine.InputSystem;


namespace Example 
{
    public static class InputActionEvents
    {
        public static Action<InputAction.CallbackContext> ToggleConsole;
        public static Action<InputAction.CallbackContext> OnExecuteCommand;
    }
}

