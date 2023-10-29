using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UpdraftShop.Asset
{
    public static class CoreAssetGuidAccessor
    {
        public static string CoreIcon => AssetGuidAccessor.GetAssetGuidReferenceGuid("CoreIcon");
    }   
}
