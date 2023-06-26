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
        [Header("Animation Feature")]
        [SerializeField] private Element[] _elements = null;

        private AnimatorOverrideController _overrideController = null;

        #region Override Functions
        protected override List<Task> GetFeatures(string state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                AnimationUIPreset.AnimationSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(PlayAnimationAsync(state.ToString(), _elements[i], setting, ct));
            }
            return features;
        }

        protected override List<Action> GetFeaturesInstant(string state)
        {
            return base.GetFeaturesInstant(state);
        }

        #endregion

        private async Task PlayAnimationAsync(string executionState, Element element, AnimationUIPreset.AnimationSetting setting, CancellationToken ct)
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
                    UIStates.Show.ToString(),
                    UIStates.Hide.ToString(),
                    UIStates.Interactive.ToString(),
                    UIStates.NonInteractive.ToString(),
                    UIStates.Press.ToString(),
                    UIStates.Click.ToString()
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

            if (executionState == UIStates.Default.ToString() || executionState == UIStates.Click.ToString())
                element.Animator.SetBool(UIStates.Hover.ToString(), false);

            await Task.Delay(TimeSpan.FromSeconds(setting.Animation.length).Milliseconds, ct);
        }

        private async Task PlayAnimationInstant()
        {

        }
    }

    [Serializable]
    public class Element
    {
        public bool IsEnabled;
        public Animator Animator = null;
        public AnimationUIPreset Preset = null;
    }
}
