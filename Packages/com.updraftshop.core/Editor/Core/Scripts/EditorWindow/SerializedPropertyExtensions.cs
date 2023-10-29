#if UNITY_EDITOR
using System;
using UnityEditor;

namespace UpdraftShop.EditorWindow.Extension
{
    public static class SerializedPropertyExtensions
    {
        public static T GetEnumValue<T>(this SerializedProperty property) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), property.enumValueIndex);
        }
    }
}
#endif