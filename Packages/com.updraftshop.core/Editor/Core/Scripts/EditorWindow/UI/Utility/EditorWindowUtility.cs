#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        private static readonly GUIStyle TitleStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };
        
        private static readonly GUIStyle CaptionStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
        };

        private static readonly GUIStyle FitLabelStyle = new GUIStyle(GUI.skin.label);
        private static readonly GUIContent FitLabelContent = new GUIContent();

            
        public static void FitLabelField(string text, TextAnchor textAnchor = TextAnchor.MiddleRight, string tooltip = "")
        {
            FitLabelStyle.alignment = textAnchor;

            FitLabelContent.text = text;
            float width = FitLabelStyle.CalcSize(FitLabelContent).x;
            GUIContent label = new GUIContent(text, tooltip);
            EditorGUILayout.LabelField(label, FitLabelStyle, GUILayout.Width(width));
        }
        
        public static void ViewTitle(string text, int fontSize = 16)
        {
            EditorGUILayout.Space();
            
            TitleStyle.fontSize = fontSize;
            TitleStyle.normal.textColor = GetDisplayColorForProSkin();

            EditorGUILayout.LabelField(text, TitleStyle);
            EditorGUILayout.Space();
        }
        
        public static void ViewCaption(string text, int fontSize = 16, TextAnchor textAnchor = TextAnchor.MiddleCenter, Color? textColor = null) 
        {
            CaptionStyle.alignment = textAnchor;
            CaptionStyle.fontSize = fontSize;
            CaptionStyle.normal.textColor =  textColor != null ? textColor.Value : GetDisplayColorForProSkin();

            EditorGUILayout.LabelField(text, CaptionStyle);
        }

        public static Color GetDisplayColorForProSkin()
        {
            if (EditorGUIUtility.isProSkin)
            {
                return Color.white;
            }
            else
            {
                return Color.black;
            }
        }
    }
}
#endif