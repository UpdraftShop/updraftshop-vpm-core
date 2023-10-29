using System;
using UpdraftShop.EditorWindow.Language;

namespace UpdraftShop.EditorWindow
{
    public interface IEditorLanguage
    {
        void SetLanguageData(Func<LanguageData> getLanguageDataFunc);
    }
}