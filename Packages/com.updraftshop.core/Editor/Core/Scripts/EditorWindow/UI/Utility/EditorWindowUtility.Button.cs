#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        private static GUIContent _iconButtonContent;
        private static GUIContent IconButtonContent(string tooltipText)
        {
            if (_iconButtonContent == null)
            {
                _iconButtonContent = new GUIContent()
                {
                    text = String.Empty,
                };
            }

            _iconButtonContent.tooltip = tooltipText ?? String.Empty;
            return _iconButtonContent;
        }

        private static GUIStyle _iconOnlyButtonStyle;
        private static GUIStyle IconOnlyButtonStyle()
        {
            if (_iconOnlyButtonStyle == null)
            {
                _iconOnlyButtonStyle = new GUIStyle(GUIStyle.none);
            }
            return _iconOnlyButtonStyle;
        }
        
        public enum ToggleButtonPosition
        {
            Left,
            Right,
        }

        public static bool ViewToggleButton(string caption, string tooltip, bool states, bool statesControl, ToggleButtonPosition toggleButtonPosition = ToggleButtonPosition.Right)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(!statesControl))
                {
                    var label = new GUIContent(string.Empty, tooltip);
                    var isResult = false;

                    switch (toggleButtonPosition)
                    {
                        case ToggleButtonPosition.Right:
                            EditorGUILayout.PrefixLabel(caption);
                            isResult = GUILayout.Toggle(states, label, GUILayout.ExpandWidth(false));
                            break;

                        case ToggleButtonPosition.Left:
                            isResult = GUILayout.Toggle(states, label, GUILayout.ExpandWidth(false));
                            EditorGUILayout.PrefixLabel(caption);
                            break;
                    }

                    return isResult;
                }
            }
        }

        public static bool ViewIconButton(int buttonWidth, int buttonHeight, Texture2D iconTexture, 
            int iconWidth, int iconHeight, float adjustIconRectX = 0.0f, float adjustIconRectY = 0.0f, string tooltipText = null)
        {
            if (GUILayout.Button(IconButtonContent(tooltipText), GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                return true;
            }
                        
            Rect buttonRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(new Rect(buttonRect.x + adjustIconRectX, buttonRect.y + adjustIconRectY, iconWidth, iconHeight), iconTexture);

            return false;
        }

        public static bool ViewIconOnlyButton(int buttonWidth, int buttonHeight, Texture2D iconTexture, int iconWidth,
            int iconHeight, float adjustIconRectX = 0.0f, float adjustIconRectY = 0.0f, string tooltipText = null)
        {
            if (GUILayout.Button(IconButtonContent(tooltipText), IconOnlyButtonStyle(), GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)))
            {
                return true;
            }
            Rect buttonRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(new Rect(buttonRect.x + adjustIconRectX, buttonRect.y + adjustIconRectY, iconWidth, iconHeight), iconTexture);
            
            return false;
        }
    }
}
#endif