using System;
using System.Collections.Generic;
using UnityEngine;

namespace ORMish
{
    public static class ThemeSelectionEvents
    {
        public static Action<bool, UnlockableScriptableObject.ETheme, SceneState> ThemeClicked;
        public static Action<List<GameObject>> LoadThemes;
    }
}
