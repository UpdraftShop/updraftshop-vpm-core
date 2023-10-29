#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UpdraftShop.EditorWindow.Utility
{
    public static partial class EditorWindowUtility
    {
        public static void DrawReadOnlyGameObjectField(GameObject drawObject)
        {
            // GameObjectを変更できないObjectFieldを表示する
            GUI.enabled = false;
            EditorGUILayout.ObjectField(drawObject, typeof(GameObject), true);
            GUI.enabled = true;

            // クリックしたらSceneViewでGameObjectをフォーカスする
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && drawObject != null)
            {
                Selection.activeGameObject = drawObject;
            }
        }
    }
}
#endif