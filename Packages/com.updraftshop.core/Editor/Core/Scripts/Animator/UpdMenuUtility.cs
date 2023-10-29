#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace UpdraftShop.Core.Animator
{
    public static class UpdMenuUtility
    {
        public static VRCExpressionsMenu CreateMenu(string name, string assetPath)
        {
            if (!Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }

            // VRCExpressionMenu アセットを作成
            VRCExpressionsMenu expressionMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();

            // アセットを保存
            var path = $"{assetPath}/{name}.asset";
            AssetDatabase.CreateAsset(expressionMenu, path);
            AssetDatabase.SaveAssets();

            return expressionMenu;
        }
    }
}
#endif