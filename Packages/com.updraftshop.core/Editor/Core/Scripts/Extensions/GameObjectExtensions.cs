using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop
{
    public static class GameObjectExtension
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }
    }    
}