#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UpdraftShop.Core.Language;

namespace UpdraftShop.Core.Animator
{
    public static class UpdAnimatorUtility
    {
        public static AnimatorController CreateAnimator(string name, string assetPath, AnimationLanguageData languageData)
        {
            if (!Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }
            
            var path = $"{assetPath}/{name}.controller";
            if (File.Exists(path))
            {
                if (!EditorUtility.DisplayDialog(languageData.OverwriteConfirmation, string.Format(languageData.OverwriteConfirmationMessage, path), languageData.Ok, languageData.Cancel))
                {
                    return null;
                }
            }
            
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            AssetDatabase.SaveAssets();

            return controller;
        }
        
        public static AnimatorController DuplicateAnimatorController(AnimatorController sourceController, string assetPath)
        {
            var duplicatePath = assetPath + $"/{sourceController.name} (Clone).controller";
            
            // フォルダ確認
            if (!Directory.Exists(assetPath))
            {
                Directory.CreateDirectory(assetPath);
            }
            
            // アセットを複製
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(sourceController), duplicatePath);
            AssetDatabase.Refresh(); // アセットのリフレッシュ

            // 複製したアセットを取得
            AnimatorController newController = AssetDatabase.LoadAssetAtPath<AnimatorController>(duplicatePath);

            return newController;
        }

        public static AnimationClip CreateAnimationClip(string assetPath)
        {
            AnimationClip newClip = new AnimationClip();
            AssetDatabase.CreateAsset(newClip, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            return newClip;
        }

        public static AnimationData[] GetAnimationData(AnimationClip animationClip)
        {
            if (animationClip == null) return null;
            var animationDataList = new List<AnimationData>();
            var curveBindings = AnimationUtility.GetCurveBindings(animationClip);
            foreach (var binding in curveBindings)
            {
                var path = binding.path;
                var propertyName = binding.propertyName;
                var animationCurve = AnimationUtility.GetEditorCurve(animationClip, binding);
                
                animationDataList.Add(new AnimationData(path, propertyName, animationCurve));
            }

            return animationDataList.ToArray();
        }
    }
    
    [Serializable]
    public class AnimationData
    {
        public string Path { get; }
        public string PropertyName { get; }
        public AnimationCurve AnimationCurve;

        public AnimationData(string path, string propertyName, AnimationCurve animationCurve)
        {
            Path = path;
            PropertyName = propertyName;
            AnimationCurve = animationCurve;
        }
    }
}
#endif