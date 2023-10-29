#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UpdraftShop
{
    public abstract class PrefabEditorSingleton<T> : EditorSingleton<T> where T : PrefabEditorSingleton<T>
    {
        private static T _instance;

        public new static T Instance
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
                        (_instance as PrefabEditorSingleton<T>)?.LoadOrCreatePrefab();
                    }
                }
                return _instance;
            }
        }

        protected virtual string PrefabSavePath => "Assets/";

        private void LoadOrCreatePrefab()
        {
            var prefabPath = $"{PrefabSavePath}/{typeof(T).Name}.prefab";
            if (IsPrefabAtPath(prefabPath))
            {
                // 既存のプレハブがある場合、新しいシーンにインスタンス化
                InstantiatePrefab(prefabPath);
                _instance = FindObjectOfType<T>();
                
                // プレハブを読み込んで自身は破棄する
                DestroyImmediate(this.gameObject);
            }
            else
            {
                // 既存のプレハブがない場合、新しいプレハブを作成してシーンにインスタンス化
                CreateNewPrefab(this.gameObject, typeof(T).Name);
                LoadOrCreatePrefab();
            }
        }

        private bool IsPrefabAtPath(string prefabPath)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            return prefab != null;
        }
        
        private GameObject InstantiatePrefab(string prefabPath)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                return instance;
            }
            else
            {
                Debug.LogWarning("Prefab not found at path: " + prefabPath);
                return null;
            }
        }
        
        private void CreateNewPrefab(GameObject obj, string prefabName)
        {
            PrefabUtility.SaveAsPrefabAsset(obj, $"{PrefabSavePath}/{typeof(T).Name}.prefab");
        }

        protected void SavePrefab()
        {
            PrefabUtility.ApplyPrefabInstance(this.gameObject, InteractionMode.AutomatedAction);
        }
    }
}
#endif