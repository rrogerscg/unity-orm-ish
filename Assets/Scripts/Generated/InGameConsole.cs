using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Example
{
    public class InGameConsole : MonoBehaviour
    {
        private Queue<string> _logQueue = new();
        private Vector2 _scrollPos;
        // this fills the height of the screen for iPad
        private const int _maxLogs = 58;
        private bool _doShowConsole = false;

        private string _commandInput = "";
        private bool _focusInputField = true;

        // Dictionary to store console commands
        private Dictionary<string, Action<string[]>> _commands = new();

        private void Start()
        {
            // Register some example commands
            RegisterCommand("help", ShowHelp);
            RegisterCommand("clear", ClearLogs);
            RegisterCommand("echo", EchoCommand);
            RegisterCommand("quit", QuitApplication);
            RegisterCommand("time", ShowTime);
            RegisterCommand("close", CloseConsole);
            RegisterCommand("c", CloseConsole);
            RegisterCommand("fps", ToggleFPS);
        }

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
            InputActions.SubscribeToToggleConsole(ToggleConsole);
            InputActions.SubscribeToExecuteCommand(OnExecuteCommand);
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
            InputActions.UnsubscribeFromToggleConsole(ToggleConsole);
            InputActions.UnsubscribeFromExecuteCommand(OnExecuteCommand);
        }

        void ToggleFPS(string[] args)
        {
            FPSCounter.Events.ToggleFPS?.Invoke();
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (_logQueue.Count > _maxLogs)
            {
                _logQueue.Dequeue();
            }
            _logQueue.Enqueue($"[{type}] {logString}");
        }

        private void ToggleConsole(InputAction.CallbackContext _)
        {
            Debug.Log("ToggleConsole called");
            _doShowConsole = !_doShowConsole;
        }

        void OnGUI()
        {
            if (!_doShowConsole) return;

            // Console window background
            GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 10), "Console");

            // Log display area
            GUILayout.BeginArea(new Rect(15, 35, Screen.width * .3f, 35));
            GUILayout.BeginHorizontal();

            GUILayout.Label(">", GUILayout.Width(20));

            // Set focus to input field when console opens
            GUI.SetNextControlName("CommandInput");
            _commandInput = GUILayout.TextField(_commandInput);

            if (_focusInputField)
            {
                GUI.FocusControl("CommandInput");
                _focusInputField = false;
            }

            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();

            // Command input area
            GUILayout.BeginArea(new Rect(35, 60, Screen.width * .2f, Screen.height - 10));
            _scrollPos = GUILayout.BeginScrollView(_scrollPos); // Fixed: removed asterisk

            foreach (var log in _logQueue.Reverse())
            {
                // Color code different log types
                GUI.color = GetLogColor(log);
                GUILayout.Label(log);
                GUI.color = Color.white;
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            // Handle Enter key for command execution
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                Event.current.Use();
            }
        }

        private void OnExecuteCommand(InputAction.CallbackContext _)
        {
            Debug.Log($"Command sent to process: {_commandInput}");
            ExecuteCommand(_commandInput);
            _commandInput = "";
            _focusInputField = true;
        }
        private Color GetLogColor(string log)
        {
            if (log.Contains("[Error]")) return Color.red;
            if (log.Contains("[Warning]")) return Color.yellow;
            if (log.Contains("[Console]")) return Color.cyan;
            return Color.white;
        }

        private void ExecuteCommand(string input)
        {
            if (string.IsNullOrEmpty(input.Trim())) return;

            // Log the command
            LogToConsole($"> {input}");

            // Parse command and arguments
            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();
            string[] args = new string[parts.Length - 1];
            Array.Copy(parts, 1, args, 0, args.Length);

            // Execute command if it exists
            if (_commands.ContainsKey(command))
            {
                try
                {
                    _commands[command](args);
                }
                catch (Exception ex)
                {
                    LogToConsole($"Error executing command '{command}': {ex.Message}");
                }
            }
            else
            {
                LogToConsole($"Unknown command: '{command}'. Type 'help' for available commands.");
            }
        }

        public void RegisterCommand(string command, Action<string[]> action)
        {
            _commands[command.ToLower()] = action;
        }

        private void LogToConsole(string message)
        {
            if (_logQueue.Count >= _maxLogs)
            {
                _logQueue.Dequeue();
            }
            _logQueue.Enqueue($"[Console] {message}");
        }

        private void ShowHelp(string[] args)
        {
            LogToConsole("Available commands:");
            foreach (var command in _commands.Keys)
            {
                LogToConsole($"  {command}");
            }
        }

        private void ClearLogs(string[] args)
        {
            _logQueue.Clear();
            LogToConsole("Console cleared.");
        }

        private void EchoCommand(string[] args)
        {
            if (args.Length > 0)
            {
                LogToConsole(string.Join(" ", args));
            }
            else
            {
                LogToConsole("Echo: No arguments provided");
            }
        }

        private void QuitApplication(string[] args)
        {
            LogToConsole("Quitting application...");
            GameManager.Instance.ExitGame();
        }

        private void ShowTime(string[] args)
        {
            LogToConsole($"Current time: {System.DateTime.Now}");
            LogToConsole($"Time since startup: {Time.realtimeSinceStartup:F2} seconds");
        }

        private void CloseConsole(string[] args)
        {
            _doShowConsole = false;
        }
    }


}
