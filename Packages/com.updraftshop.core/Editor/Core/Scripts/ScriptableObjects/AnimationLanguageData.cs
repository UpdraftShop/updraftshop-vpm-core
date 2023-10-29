using UnityEngine;
using UpdraftShop.Constant;
using UpdraftShop.EditorWindow.Language;

namespace UpdraftShop.Core.Language
{
    [CreateAssetMenu(fileName = "AnimationLanguageData", menuName = GlobalShopConstant.ShopMenuRootName + "/Develop/ScriptableObjects/Core/AnimationLanguageData")]
    public class AnimationLanguageData : LanguageData
    {
        [SerializeField]
        private string _overwriteConfirmation = string.Empty;
        public string OverwriteConfirmation => _overwriteConfirmation;

        [SerializeField]
        private string _overwriteConfirmationMessage = string.Empty;
        public string OverwriteConfirmationMessage => _overwriteConfirmationMessage;

        [SerializeField]
        private string _ok = string.Empty;
        public string Ok => _ok;

        [SerializeField]
        private string _cancel = string.Empty;
        public string Cancel => _cancel;
    }   
}
