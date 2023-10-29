using System;
using UnityEngine;
using UpdraftShop.EditorWindow.Language;

namespace UpdraftShop.EditorWindow
{
    public abstract class EditorViewBase : IEditorReset, IEditorDraw, IEditorLanguage 
    {
        protected Func<LanguageData> _getLanguageDataFunc;
        
        public virtual void Reset()
        {
        }

        public virtual void OnDraw(Rect windowRect)
        {
        }

        public void SetLanguageData(Func<LanguageData> getLanguageDataFunc)
        {
            _getLanguageDataFunc = getLanguageDataFunc;
        }
    }
}