using UnityEngine;


namespace UpdraftShop
{
    public abstract class EditorSingleton<T> : MonoBehaviour where T : EditorSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var gameObject = new GameObject(typeof(T).Name);
                        gameObject.tag = "EditorOnly";
                        _instance = gameObject.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}