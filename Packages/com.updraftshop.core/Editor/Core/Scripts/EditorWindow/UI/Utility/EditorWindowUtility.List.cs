using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        public static void AddGameObjectIfNotExists(GameObject gameObjectToAdd, ref List<GameObject> uniqueGameObjectList)
        {
            if (!uniqueGameObjectList.Contains(gameObjectToAdd))
            {
                uniqueGameObjectList.Add(gameObjectToAdd);
            }
        }
    
        public static List<GameObject> GetAllParentObject(Transform target, Transform root = null)
        {
            var parentList = new List<GameObject>();
            var parent = target.parent;
            while (parent != null && parent != root)
            {
                parentList.Add(parent.gameObject);
                parent = parent.parent;
            }
            return parentList;
        }
    }
}
