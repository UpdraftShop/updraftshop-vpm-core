using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UpdraftShop.Constant;

namespace UpdraftShop.Core.Language
{
    [CreateAssetMenu(fileName = "HelpItem", menuName = GlobalShopConstant.ShopMenuRootName + "/Develop/ScriptableObjects/Help/HelpItem")]
    public class HelpItem : ScriptableObject
    {
        [SerializeField]
        private string _itemName = string.Empty;
        public string ItemName => _itemName;
        
        [SerializeField]
        private VersionData _versionData = new VersionData();
        public VersionData VersionData => _versionData;

        [SerializeField]
        private List<PrerequisiteVersionData> _prerequisiteVersionDataList = new List<PrerequisiteVersionData>();
        public List<PrerequisiteVersionData> PrerequisiteVersionDataList => _prerequisiteVersionDataList;

        [SerializeField]
        private string _description = string.Empty;
        public string Description => _description.ReplaceEscapeCharacter();

        [SerializeField]
        private int _displayPriority = 0;
        public int DisplayPriority => _displayPriority;
        
        [SerializeField]
        private string _itemIconId = string.Empty;
        public string ItemIconId => _itemIconId;
    }

    [Serializable]
    public class VersionData
    {
        /// <summary>
        /// tool version
        /// </summary>
        /// <remarks>
        /// ver a.bc
        /// 
        /// a = 大きなアップデートで増加
        /// b = 小さなアップデートで増加
        /// c = バグ修正などで増加
        /// </remarks>
        [SerializeField]
        private string _version = "1.0.0";
        public string Version => _version;

        private int _majorVersion = 0;
        public int MajorVersion
        {
            get
            {
                ParseVersion();
                return _majorVersion;
            }
        }
        
        private int _minorVersion = 0;
        public int MinorVersion
        {
            get
            {
                ParseVersion();
                return _minorVersion;
            }
        } 
        
        private int _patchVersion = 0;
        public int PatchVersion
        {
            get
            {
                ParseVersion();
                return _patchVersion;
            }
        }
        
        private void ParseVersion()
        {
            string[] parts = _version.Split('.');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid version format.");
            }

            _majorVersion = int.Parse(parts[0]);
            _minorVersion = int.Parse(parts[1]);
            _patchVersion = int.Parse(parts[2]);
        }
    }

    [Serializable]
    public class PrerequisiteVersionData
    {
        [SerializeField]
        private string _prerequisiteItemName = string.Empty;
        public string PrerequisiteItemName => _prerequisiteItemName;
        
        [SerializeField]
        private VersionData _versionData = new VersionData();
        public VersionData VersionData => _versionData;
    }
}
