#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        public static void DrawLine(Color? color = null, int lineSize = 1, float? lineWidth = null, 
            int rectOffsetLeft = 0, int rectOffsetRight = 0, int rectOffsetTop = 0, int rectOffsetBottom = 0)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color ?? Color.white);
            tex.Apply();

            var style = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = tex
                },
                margin = new RectOffset(0 + rectOffsetLeft, 0 + rectOffsetRight, 4 + rectOffsetTop, 4 + rectOffsetBottom),
                fixedHeight = lineSize,
            };
            if (lineWidth.HasValue)
            {
                style.fixedWidth = lineWidth.Value;
            }

            GUILayout.Label("", style);
        }
        
        public static void DrawOutline(Rect rect)
        {
            const int DefaultOutlineWidth = 1;
            DrawOutline(rect, DefaultOutlineWidth, EditorWindowUtility.GetDisplayColorForProSkin());
        }

        public static void DrawOutline(Rect rect, int outlineWidth)
        {
            DrawOutline(rect, outlineWidth, EditorWindowUtility.GetDisplayColorForProSkin());
        }

        public static void DrawOutline(Rect rect, int outlineWidth, Color outlineColor)
        {
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, outlineWidth), outlineColor);                    // up
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMax - outlineWidth, rect.width, outlineWidth), outlineColor);   // down
            EditorGUI.DrawRect(new Rect(rect.xMin, rect.yMin, outlineWidth, rect.height), outlineColor);                        // left
            EditorGUI.DrawRect(new Rect(rect.xMax - outlineWidth, rect.yMin, outlineWidth, rect.height), outlineColor);       // right
        }
    }
}
#endif