#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UpdraftShop.Core.Animator
{
    public static class UpdAnimatorExtension
    {
        public static readonly string VRChatParameterGestureLeft = "GestureLeft";
        public static readonly string VRChatParameterGestureLeftWeight = "GestureLeftWeight";
        public static readonly string VRChatParameterGestureRight = "GestureRight";
        public static readonly string VRChatParameterGestureRightWeight = "GestureRightWeight";
        
        private static HashSet<AnimatorControllerParameter> VRChatGestureParameterSet =
            new HashSet<AnimatorControllerParameter>()
            {
                new AnimatorControllerParameter(){ name = VRChatParameterGestureLeft, type = AnimatorControllerParameterType.Int},
                new AnimatorControllerParameter(){ name = VRChatParameterGestureLeftWeight, type = AnimatorControllerParameterType.Float},
                new AnimatorControllerParameter(){ name = VRChatParameterGestureRight, type = AnimatorControllerParameterType.Int},
                new AnimatorControllerParameter(){ name = VRChatParameterGestureRightWeight, type = AnimatorControllerParameterType.Float},
            };

        public static AnimatorControllerLayer AddDefaultStateMachine(this AnimatorControllerLayer layer, string animatorControllerAssetPath)
        {
            if (layer.stateMachine != null) return layer;
            
            // create stateMachine
            var stateMachine = new AnimatorStateMachine
            {
                name = layer.name,
                hideFlags = HideFlags.HideInHierarchy,
                
                // reposition default state
                entryPosition = new Vector3(40.0f, 100.0f, 20.0f),
                anyStatePosition = new Vector3(40.0f, 0.0f, 20.0f),
                exitPosition = new Vector3(40.0f, 200.0f, 20.0f)
            };
            
            AssetDatabase.AddObjectToAsset(stateMachine, animatorControllerAssetPath);
            //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(stateMachine));

            layer.stateMachine = stateMachine;
            return layer;
        }

        public static AnimatorState AddState(this AnimatorControllerLayer layer, string stateName, Vector3 position, AnimationClip animationClip = null, bool writeDefaultValues = true)
        {
            if (layer.stateMachine == null) return null;

            var stateMachine = layer.stateMachine;
            var state = stateMachine.AddState(stateName, position);
            state.writeDefaultValues = writeDefaultValues;
            state.motion = animationClip;
            
            return state;
        }
        
        public static AnimatorState AddState(this AnimatorStateMachine stateMachine, string stateName, Vector3 position, AnimationClip animationClip = null, bool writeDefaultValues = true)
        {
            if (stateMachine == null) return null;

            var state = stateMachine.AddState(stateName, position);
            state.writeDefaultValues = writeDefaultValues;
            state.motion = animationClip;

            return state;
        }

        public static AnimatorStateTransition SetExitTime(this AnimatorStateTransition transition, float exitTime)
        {
            transition.exitTime = exitTime;
            return transition;
        }
        
        public static AnimatorStateTransition SetDuration(this AnimatorStateTransition transition, float duration)
        {
            transition.duration = duration;
            return transition;
        }
        
        public static AnimatorStateTransition SetOffset(this AnimatorStateTransition transition, float offset)
        {
            transition.offset = offset;
            return transition;
        }

        public static void InitVRChatGestureParameter(this AnimatorController animatorController)
        {
            var parameters = animatorController.parameters;
            foreach (var set in VRChatGestureParameterSet)
            {
                if (parameters.Contains(set)) continue;
                animatorController.AddParameter(set);
            }
        }
    }
}
#endif