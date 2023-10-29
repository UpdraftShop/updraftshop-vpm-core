using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UpdraftShop.Asset
{
    public static class AssetGuidAccessor
    {
        private static readonly List<AssetGuidReference> _assetGuidReferenceList = new List<AssetGuidReference>();
        private static readonly Dictionary<string, AssetGUIDReferenceData> _assetGuidReferenceCacheMap = new Dictionary<string, AssetGUIDReferenceData>();
        private static List<AssetGuidReference> AssetGuidReferenceList
        {
            get
            {
                _assetGuidReferenceList.Clear();
            
                var guids = AssetDatabase.FindAssets($"t:{typeof(AssetGuidReference)}");
                if (guids.Length <= 0)
                {
                    throw new System.IO.FileNotFoundException($"{typeof(AssetGuidReference)} does not found.");
                }
                
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    _assetGuidReferenceList.Add(AssetDatabase.LoadAssetAtPath<AssetGuidReference>(path));   
                }

                return _assetGuidReferenceList;
            }
        }

        public static string GetAssetGuidReferenceGuid(string id)
        {
            if (_assetGuidReferenceCacheMap.TryGetValue(id, out var cache))
            {
                return cache.TargetAssetGuid;
            }
            
            foreach (var assetGuidReference in AssetGuidReferenceList)
            {
                foreach (var assetGuidReferenceData in assetGuidReference.AssetGuidReferenceDataList)
                {
                    if (assetGuidReferenceData.Id == id)
                    {
                        _assetGuidReferenceCacheMap[id] = assetGuidReferenceData;
                        
                        var guid = assetGuidReferenceData.TargetAssetGuid;
                        return guid;
                    }
                }
            }

            return string.Empty;
        }
    }   
}
