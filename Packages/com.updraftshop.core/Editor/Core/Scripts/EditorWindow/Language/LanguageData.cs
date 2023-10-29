using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop.EditorWindow.Language
{
    public abstract class LanguageData : ScriptableObject
    {
        #region Variable
        [SerializeField]
        private LanguageType _languageType;
        public LanguageType GetLanguageType() => _languageType;
        #endregion

        #region enum
        public enum LanguageType
        {
            Japanese,
            English,
            Korean,
        }
        #endregion
    }

    public static class LanguageTypeExtension
    {
        private static readonly Dictionary<LanguageData.LanguageType, string> _languageAcronym = new Dictionary<LanguageData.LanguageType, string>()
            {
                { LanguageData.LanguageType.Japanese, "JP" },
                { LanguageData.LanguageType.English, "EN" },
                { LanguageData.LanguageType.Korean, "KO" },
            };

        public static string ToAcronym(this LanguageData.LanguageType languageType)
        {
            return _languageAcronym[languageType];
        }
    }
}