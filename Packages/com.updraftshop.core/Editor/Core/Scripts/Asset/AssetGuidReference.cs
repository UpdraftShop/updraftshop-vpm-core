using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UpdraftShop.Attriburte;
using UpdraftShop.Constant;
using Object = UnityEngine.Object;

namespace UpdraftShop.Asset
{
    [CreateAssetMenu(fileName = "AssetGuidReference", menuName = GlobalShopConstant.ShopMenuRootName + "/Develop/ScriptableObjects/AssetGuidReference")]
    public class AssetGuidReference : ScriptableObject
    {
        [SerializeField]
        private List<AssetGUIDReferenceData> _assetGuidReferenceDataList = new List<AssetGUIDReferenceData>();
        public List<AssetGUIDReferenceData> AssetGuidReferenceDataList => _assetGuidReferenceDataList;


#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var assetGuidReferenceData in _assetGuidReferenceDataList)
            {
                if (assetGuidReferenceData.TargetAsset)
                {
                    string assetPath = AssetDatabase.GetAssetPath(assetGuidReferenceData.TargetAsset);
                    assetGuidReferenceData.TargetAssetGuid = AssetDatabase.AssetPathToGUID(assetPath);
                }
                else
                {
                    assetGuidReferenceData.TargetAssetGuid = string.Empty;
                }
            }
        }
#endif
    }

    [Serializable]
    public class AssetGUIDReferenceData
    {
        [SerializeField] 
        private string _id = string.Empty;
        public Object TargetAsset;
        [ReadOnly]
        public string TargetAssetGuid;

        public string Id => _id;
    }
}