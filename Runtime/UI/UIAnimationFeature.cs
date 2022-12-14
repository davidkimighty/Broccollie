using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollieMollie.Audio;
using CollieMollie.Helper;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIAnimationFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        #endregion

        #region Public Functions
        public override async Task ExecuteAsync(string state, Action done = null)
        {
            if (!_isEnabled) return;

            List<Task> executions = new List<Task>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;

                UIAnimationPreset.Setting setting = Array.Find(element.Preset.States, x => x.ExecutionState.ToString() == state);
                if (IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    executions.Add(element.PlayAnimation(state, setting));
                }
            }
            await Task.WhenAll(executions);
            done?.Invoke();
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public Animator Animator = null;
            public UIAnimationPreset Preset = null;

            private AnimatorOverrideController _overrideController = null;

            public async Task PlayAnimation(string executionState, UIAnimationPreset.Setting setting)
            {
                if (_overrideController == null)
                {
                    _overrideController = new AnimatorOverrideController(Preset.OverrideAnimator);
                    Animator.runtimeAnimatorController = _overrideController;
                }

                AnimatorOverrideController animator = (AnimatorOverrideController)Animator.runtimeAnimatorController;
                if (animator[executionState] != setting.Animation)
                {
                    animator[executionState] = setting.Animation;
                    Animator.runtimeAnimatorController = animator;
                }

                if (executionState != BaseUI.State.Hovered.ToString())
                {
                    List<string> animationStates = new List<string>();
                    animationStates.Add(BaseUI.State.Pressed.ToString());
                    animationStates.Add(BaseUI.State.Selected.ToString());
                    animationStates.Add(BaseUI.State.Default.ToString());
                    animationStates.Add(BaseUI.State.NonInteractive.ToString());
                    animationStates.Add(BaseUI.State.Show.ToString());
                    animationStates.Add(BaseUI.State.Hide.ToString());

                    foreach (string animationState in animationStates)
                    {
                        if (animationState == executionState) continue;
                        Animator.SetBool(animationState, false);
                    }
                    Animator.SetBool(setting.ExecutionState.ToString(), true);
                }
                else Animator.SetBool(BaseUI.State.Hovered.ToString(), true);

                if (executionState == BaseUI.State.Default.ToString() || executionState == BaseUI.State.Selected.ToString())
                    Animator.SetBool(BaseUI.State.Hovered.ToString(), false);

                await Task.Delay(TimeSpan.FromSeconds(setting.Animation.length).Milliseconds);
            }
        }
    }
}
