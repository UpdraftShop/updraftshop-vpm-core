using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        private static readonly RectOffset OutlineBorder = new RectOffset(5, 5, 5, 5);
        
        private static readonly Dictionary<(Color, Color), GUIStyle> _backgroundGUIStyleCacheMap = new Dictionary<(Color, Color), GUIStyle>();
        private static readonly Dictionary<(Color, Color), Texture2D> _backGroundTextureCacheMap = new Dictionary<(Color, Color), Texture2D>();
        
        public static GUIStyle GetBackgroundStyle(Color backgroundColor, Color outlineColor)
        {
            GUIStyle style = null;
            if (_backgroundGUIStyleCacheMap.TryGetValue((backgroundColor, outlineColor), out var cache))
            {
                if (cache != null && cache.normal.background != null)
                {
                    style = cache;
                }
            }

            if (style == null)
            {
                style = new GUIStyle();
                style.normal.background = GetOrMakeBackgroundTexture(backgroundColor, outlineColor);
                style.border = OutlineBorder;

                _backgroundGUIStyleCacheMap[(backgroundColor, outlineColor)] = style;
            }

            return style;
        }

        private static Texture2D GetOrMakeBackgroundTexture(Color backgroundColor, Color outlineColor)
        {
            if (_backGroundTextureCacheMap.TryGetValue((backgroundColor, outlineColor), out var texture))
            {
                if (texture != null)
                {
                    return texture;
                }
            }
            
            int width = 100; // 背景の幅
            int height = 100; // 背景の高さ

            Color[] pix = new Color[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = backgroundColor;
                    if (x < 2 || y < 2 || x >= width - 2 || y >= height - 2)
                    {
                        color = outlineColor;
                    }
                    pix[y * width + x] = color;
                }
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            
            _backGroundTextureCacheMap[(backgroundColor, outlineColor)] = result;
            return result;
        }
    }
}
