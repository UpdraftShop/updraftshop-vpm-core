using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UpdraftShop.Asset;
using UpdraftShop.Constant;
using UpdraftShop.Core.Language;
using UpdraftShop.EditorWindow;
using UpdraftShop.EditorWindow.Utility;

namespace UpdraftShop.Core.Help
{
    public sealed class HelpMenu : LanguageEditorWindow
    {
        private const string ItemName = "Help";

        private readonly Color ListBackgroundColor = new Color(0.2f, 0.2f, 0.2f);
        private readonly Color InfoBackgroundColor = new Color(0.3f, 0.3f, 0.3f);

        #region GUIStyle
        private GUIStyle _itemNameStyle = null;
        private GUIStyle ItemNameStyle =>
            _itemNameStyle ?? (_itemNameStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 24,
                normal =
                {
                    textColor = Color.white
                },
                fontStyle = FontStyle.Bold,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                fixedHeight = 0,
                fixedWidth = 0,
                stretchHeight = true,
                stretchWidth = true,
            });
        
        private GUIStyle _versionStyle = null;
        private GUIStyle VersionStyle =>
            _versionStyle ?? (_versionStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 12,
                normal =
                {
                    textColor = Color.yellow
                },
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 5, 5),
            });
        
        private GUIStyle _descriptionItemNameStyle = null;
        private GUIStyle DescriptionItemNameStyle =>
            _descriptionItemNameStyle ?? (_descriptionItemNameStyle = new GUIStyle(EditorStyles.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold,
            });
        
        private GUIStyle _descriptionStyle = null;
        private GUIStyle DescriptionStyle =>
            _descriptionStyle ?? (_descriptionStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                richText = true,
            });
        
        private GUIStyle _prerequisiteVersionStyle = null;
        private GUIStyle PrerequisiteVersionStyle =>
            _prerequisiteVersionStyle ?? (_prerequisiteVersionStyle = new GUIStyle(EditorStyles.label)
            {
                normal =
                {
                    textColor = new Color(0.75f, 0.0f, 0.0f),
                },
                wordWrap = true,
                richText = true,
            });
        #endregion

        private Dictionary<string, Texture2D> _itemIconCacheMap = new Dictionary<string, Texture2D>();
        private List<HelpItem> _helpItemList = new List<HelpItem>();
        private HelpItem _currentHelpItem = null;
        
        private Vector2 _scrollPosition = Vector2.zero;
        
        private const float ListItemWidth = 275;
        private static readonly Vector2 WindowSize = new Vector2(575, 300);
        
        [MenuItem(GlobalShopConstant.ShopMenuRootName + "/" + ItemName, false, 10000)]
        private static void Open()
        {
            var window = (HelpMenu)UnityEditor.EditorWindow.GetWindow(typeof(HelpMenu), false);
            string assetPath = AssetDatabase.GUIDToAssetPath(CoreAssetGuidAccessor.CoreIcon);
            var image = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
            window.titleContent = new GUIContent(ItemName, image);
            window.minSize = WindowSize;
            window.maxSize = WindowSize;
            
            var rect = window.position;
            rect.width = WindowSize.x;
            rect.height = WindowSize.y;
            window.position = rect;
            
            window.Show();
        }

        protected override void OnEnable()
        {
            SearchHelpItem();
        }

        private void OnGUI()
        {
            float windowWidth = position.width;

            Rect itemListRect = new Rect(0, 0, ListItemWidth, position.height);
            DrawItemList(itemListRect);

            Rect itemInfoRect = new Rect(ListItemWidth, 0, windowWidth - ListItemWidth, position.height);
            DrawItemInfo(itemInfoRect);
        }

        private void DrawItemList(Rect rect)
        {
            GUILayout.BeginArea(rect);
            var backGroundStyle = EditorWindowUtility.GetBackgroundStyle(ListBackgroundColor, ListBackgroundColor);
            using (new EditorGUILayout.VerticalScope(backGroundStyle, GUILayout.Width(rect.width), GUILayout.Height(rect.height)))
            {
                foreach (var helpItem in _helpItemList)
                {
                    EditorGUILayout.Space();
                    var buttonRect = new Rect();
                    var lastRect = GUILayoutUtility.GetLastRect();
                    buttonRect.position = lastRect.position;

                    EditorGUILayout.LabelField($"{helpItem.ItemName}", ItemNameStyle, GUILayout.ExpandWidth(false));
                    EditorGUILayout.LabelField($"v{helpItem.VersionData.Version}", VersionStyle, GUILayout.ExpandWidth(false));
                    EditorGUILayout.Space();
                    
                    lastRect = GUILayoutUtility.GetLastRect();
                    buttonRect.width = rect.width;
                    buttonRect.height = lastRect.position.y - buttonRect.y + lastRect.height;

                    if (GUI.Button(buttonRect, "", new GUIStyle(GUIStyle.none)))
                    {
                        _currentHelpItem = helpItem;
                        _scrollPosition = Vector2.zero;
                    }
                    
                    // icon
                    if (!string.IsNullOrEmpty(helpItem.ItemIconId))
                    {
                        float iconWidth = 45.0f;
                        float iconHeight = 45.0f;
                        Rect iconRect = new Rect(rect.width - iconWidth, buttonRect.position.y, iconWidth, iconHeight);
                        string assetPath = AssetDatabase.GUIDToAssetPath(AssetGuidAccessor.GetAssetGuidReferenceGuid(helpItem.ItemIconId));
                        GUI.DrawTexture(iconRect, GetIconTexture(assetPath), ScaleMode.ScaleToFit);
                    }
                    
                    EditorWindowUtility.DrawLine();
                }
            }
            GUILayout.EndArea();
        }

        private void DrawItemInfo(Rect rect)
        {
            GUILayout.BeginArea(rect);
            var infoGroundStyle = EditorWindowUtility.GetBackgroundStyle(InfoBackgroundColor, InfoBackgroundColor);
            using (new EditorGUILayout.VerticalScope(infoGroundStyle, GUILayout.ExpandWidth(true), GUILayout.Height(rect.height)))
            {
                // アイテム名
                EditorGUILayout.LabelField(_currentHelpItem.ItemName, DescriptionItemNameStyle);
                
                // 前提バージョン
                var prerequisiteVersionDataList = _currentHelpItem.PrerequisiteVersionDataList;
                if (prerequisiteVersionDataList != null && prerequisiteVersionDataList.Count > 0)
                {
                    string prerequisiteText = "※必須";
                    
                    foreach (var prerequisiteVersionData in prerequisiteVersionDataList)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            prerequisiteText += $"\n{prerequisiteVersionData.PrerequisiteItemName} {prerequisiteVersionData.VersionData.Version}";
                            prerequisiteText += $" <color=yellow>{(MeetThePrerequisiteVersionConditions(prerequisiteVersionData)? "インストール済み" : "未インストール")}</color>";
                        }   
                    }
                    
                    EditorGUILayout.LabelField(prerequisiteText, PrerequisiteVersionStyle);
                }
                
                EditorGUILayout.Space();
                
                // 説明
                using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
                {
                    EditorGUILayout.LabelField(_currentHelpItem.Description, DescriptionStyle);
                    _scrollPosition = scroll.scrollPosition;
                }
            }
            GUILayout.EndArea();
        }

        private void SearchHelpItem()
        {
            // Search Unity Assets
            var guids = AssetDatabase.FindAssets($"t:{nameof(HelpItem)}");
            if (guids.Length <= 0)
            {
                throw new System.IO.FileNotFoundException($"{nameof(HelpItem)} does not found.");
            }

            // Add HelpItem
            _helpItemList.Clear();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var findData = AssetDatabase.LoadAssetAtPath<HelpItem>(path);

                _helpItemList.Add(findData);
            }
            _helpItemList = _helpItemList.OrderBy(_ => _.DisplayPriority).ToList();
            _currentHelpItem = _helpItemList[0];
        }

        private Texture2D GetIconTexture(string path)
        {
            Texture2D texture;
            if (_itemIconCacheMap.TryGetValue(path, out texture) && texture != null)
            {
                return texture;
            }

            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            _itemIconCacheMap[path] = texture;
            
            return texture;
        }

        private bool MeetThePrerequisiteVersionConditions(PrerequisiteVersionData prerequisiteVersionData)
        {
            var targetVersionData = _helpItemList.FirstOrDefault(_ => _.ItemName == prerequisiteVersionData.PrerequisiteItemName);
            if (targetVersionData == null)
            {
                return false;
            }
            if (targetVersionData.VersionData.MajorVersion < prerequisiteVersionData.VersionData.MajorVersion)
            {
                return false;
            }
            var isSameMajorVersion = targetVersionData.VersionData.MajorVersion == prerequisiteVersionData.VersionData.MajorVersion;
            if (isSameMajorVersion && targetVersionData.VersionData.MinorVersion < prerequisiteVersionData.VersionData.MinorVersion)
            {
                return false;
            }
            var isSameMinorVersion = targetVersionData.VersionData.MinorVersion == prerequisiteVersionData.VersionData.MinorVersion;
            if (isSameMajorVersion && isSameMinorVersion && targetVersionData.VersionData.PatchVersion < prerequisiteVersionData.VersionData.PatchVersion)
            {
                return false;
            }
            
            return true;
        }
    }    
}