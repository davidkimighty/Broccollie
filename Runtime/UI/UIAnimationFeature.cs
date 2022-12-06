using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Audio;
using CollieMollie.Core;
using CollieMollie.Helper;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIAnimationFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        private Operation _featureOperation = new Operation();

        #endregion

        #region Public Functions
        public override void Execute(string state, out float duration, Action done = null)
        {
            duration = 0;
            if (!_isEnabled) return;

            _featureOperation.Stop(this);
            List<float> durations = new List<float>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                _featureOperation.Add(element.PlayAnimation(state));
                durations.Add(element.Preset.GetDuration(state));
            }
            duration = durations.Count > 0 ? durations.Max() : 0;
            _featureOperation.Start(this, duration, done);
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public Animator Animator = null;
            public UIAnimationPreset Preset = null;

            private AnimatorOverrideController _overrideController = null;

            public IEnumerator PlayAnimation(string executionState)
            {
                if (_overrideController == null)
                {
                    _overrideController = new AnimatorOverrideController(Preset.OverrideAnimator);
                    Animator.runtimeAnimatorController = _overrideController;
                }

                UIAnimationPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == executionState);
                if (Preset.IsValid(setting.ExecutionState))
                {
                    if (!setting.IsEnabled) yield break;

                    AnimatorOverrideController animator = (AnimatorOverrideController)Animator.runtimeAnimatorController;
                    if (animator[executionState] != setting.Animation)
                    {
                        animator[executionState] = setting.Animation;
                        Animator.runtimeAnimatorController = animator;
                    }

                    if (executionState != UIInteractionState.Hovered.ToString())
                    {
                        List<string> animationStates = new List<string>();
                        animationStates.Add(UIInteractionState.Pressed.ToString());
                        animationStates.Add(UIInteractionState.Selected.ToString());
                        animationStates.Add(UIState.Default.ToString());
                        animationStates.Add(UIState.NonInteractive.ToString());
                        animationStates.Add(UIState.Show.ToString());
                        animationStates.Add(UIState.Hide.ToString());

                        foreach (string animationState in animationStates)
                        {
                            if (animationState == executionState) continue;
                            Animator.SetBool(animationState, false);
                        }
                        Animator.SetBool(setting.ExecutionState.ToString(), true);
                    }
                    else Animator.SetBool(UIInteractionState.Hovered.ToString(), true);

                    if (executionState == UIState.Default.ToString() || executionState == UIInteractionState.Selected.ToString())
                        Animator.SetBool(UIInteractionState.Hovered.ToString(), false);
                }
            }
        }
    }
}
