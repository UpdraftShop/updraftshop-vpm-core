using System;
using UnityEngine;

namespace UpdraftShop.Core.Path
{
    public static class UpdPathUtility
    {
        public static string GetRelativeAssetPath(string absolutePath)
        {
            string projectPath = Application.dataPath;
            string relativePath = "Assets" + absolutePath.Substring(projectPath.Length);
            
            return relativePath;
        }
        
        public static string GetAbsolutePath(string assetPath)
        {
            string fullPath = Application.dataPath;
            fullPath = fullPath.Remove(fullPath.LastIndexOf("/Assets", StringComparison.Ordinal));
            fullPath += assetPath;

            return fullPath;
        }
    }    
}