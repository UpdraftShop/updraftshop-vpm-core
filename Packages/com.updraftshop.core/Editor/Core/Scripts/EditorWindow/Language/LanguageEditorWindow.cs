#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UpdraftShop.EditorWindow.Language;

namespace UpdraftShop.EditorWindow
{
    public abstract class LanguageEditorWindow : UnityEditor.EditorWindow
    {
        #region Constant
        private const string ActiveLanguageColorTag = "<color=orange>";
        private const string DeactiveLanguageColorTag = "<color=grey>";
        private string DefaultColorTag => EditorGUIUtility.isProSkin? "<color=white>" : "<color=black>";
        private const string ColorTagEnd = "</color>";
        #endregion

        #region Variable
        private static LanguageData.LanguageType _currentLanguageType = LanguageData.LanguageType.Japanese;
        private static LanguageData.LanguageType CurrentLanguageType => _currentLanguageType;

        private static Dictionary<Type, LanguageData> _cacheLanguageDataMap = new Dictionary<Type, LanguageData>();
        #endregion


        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnDestroy()
        {

        }

        protected bool ViewLanguageSwitchButton()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var style = new GUIStyle(EditorStyles.miniButtonRight);
                style.richText = true;

                System.Text.StringBuilder languageTextBuilder = new System.Text.StringBuilder();
                System.Text.StringBuilder simpleTextBuilder = new System.Text.StringBuilder();

                var languageTypes = (LanguageData.LanguageType[])Enum.GetValues(typeof(LanguageData.LanguageType));
                var acronymTexts = new string[languageTypes.Length];
                var simpleAcronymTexts = new string[languageTypes.Length];
                for (int i = 0; i < languageTypes.Length; i++)
                {
                    var language = languageTypes[i];
                    acronymTexts[i] = $"{(CurrentLanguageType == language ? ActiveLanguageColorTag : DeactiveLanguageColorTag)}{language.ToAcronym()}{ColorTagEnd}";
                    simpleAcronymTexts[i] = $"{language.ToAcronym()}";
                }
                languageTextBuilder.Append(string.Join($"{DefaultColorTag}/{ColorTagEnd}", acronymTexts));
                simpleTextBuilder.Append(string.Join("/", simpleAcronymTexts));

                GUILayout.FlexibleSpace();
                var width = Mathf.Max(40, GUI.skin.label.CalcSize(new GUIContent(simpleTextBuilder.ToString())).x + 20);
                var buttonResult = GUILayout.Button(languageTextBuilder.ToString(), style, GUILayout.Width(width));

                return buttonResult;

            }
        }

        protected void SwitchLanguage()
        {
            var languages = (LanguageData.LanguageType[])Enum.GetValues(typeof(LanguageData.LanguageType));
            int currentIndex = Array.IndexOf(languages, CurrentLanguageType);
            int nextIndex = (currentIndex + 1) % languages.Length;

            _currentLanguageType = languages[nextIndex];
        }

        public T GetLanguageFromType<T>() where T : LanguageData
        {
            var type = typeof(T);

            // return cache.
            if (_cacheLanguageDataMap.TryGetValue(type, out var cacheData)) {
                if (cacheData is T result && result.GetLanguageType() == CurrentLanguageType)
                {
                    return result;
                }
                else
                {
                    // not match current language type.
                    _cacheLanguageDataMap.Remove(type);
                }
            }

            // Search Unity Assets
            var guids = AssetDatabase.FindAssets($"t:{type.Name}");
            if (guids.Length <= 0)
            {
                throw new System.IO.FileNotFoundException($"{type.Name} does not found.");
            }

            // Select LanguageType
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var findData = AssetDatabase.LoadAssetAtPath<LanguageData>(path);

                if (findData is T result && findData.GetLanguageType() == CurrentLanguageType)
                {
                    _cacheLanguageDataMap.Add(type, result);
                    return result;
                }
            }

            return null;
        }
    }
}
#endif