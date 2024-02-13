using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    [DisallowMultipleComponent]
    public class AnimationUIFeature : BaseUIFeature
    {
        [SerializeField] private Element[] _elements = null;

        private AnimatorOverrideController _overrideController = null;

        #region public Functions
        public override List<Task> GetFeatures(UIStates state, bool instantChange, bool playAudio, CancellationToken ct)
        {
            try
            {
                if (_elements == null) return default;

                List<Task> features = new();
                for (int i = 0; i < _elements.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                    AnimationUIFeaturePreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                    if (!setting.IsEnabled) continue;

                    features.Add(PlayAnimationAsync(state.ToString(), _elements[i], setting, ct));
                }
                return features;
            }
            catch (OperationCanceledException e)
            {
                return default;
            }
        }

        #endregion

        private async Task PlayAnimationAsync(string executionState, Element element, AnimationUIFeaturePreset.Setting setting, CancellationToken ct)
        {
            if (_overrideController == null)
            {
                _overrideController = new AnimatorOverrideController(element.Preset.OverrideAnimator);
                element.Animator.runtimeAnimatorController = _overrideController;
            }

            AnimatorOverrideController animator = (AnimatorOverrideController)element.Animator.runtimeAnimatorController;
            if (animator[executionState] != setting.Animation)
            {
                animator[executionState] = setting.Animation;
                element.Animator.runtimeAnimatorController = animator;
            }

            if (executionState != UIStates.Hover.ToString())
            {
                List<string> animationStates = new List<string>
                {
                    UIStates.Default.ToString(),
                    UIStates.Active.ToString(),
                    UIStates.InActive.ToString(),
                    UIStates.Interactive.ToString(),
                    UIStates.NonInteractive.ToString(),
                    UIStates.Press.ToString(),
                    UIStates.Select.ToString()
                };

                foreach (string animationState in animationStates)
                {
                    if (animationState == executionState) continue;
                    element.Animator.SetBool(animationState, false);
                }
                element.Animator.SetBool(setting.ExecutionState.ToString(), true);
            }
            else
            {
                element.Animator.SetBool(UIStates.Press.ToString(), false);
                element.Animator.SetBool(UIStates.Default.ToString(), true);
                element.Animator.SetBool(UIStates.Hover.ToString(), true);
            }

            if (executionState == UIStates.Default.ToString() || executionState == UIStates.Select.ToString())
                element.Animator.SetBool(UIStates.Hover.ToString(), false);

            await Task.Delay(TimeSpan.FromSeconds(setting.Animation.length).Milliseconds, ct);
        }
    }

    [Serializable]
    public struct Element
    {
        public bool IsEnabled;
        public Animator Animator;
        public AnimationUIFeaturePreset Preset;
    }
}
