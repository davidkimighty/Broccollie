using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UISpriteFeature : BaseUIFeature
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
                _featureOperation.Add(element.ChangeSprite(state));
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
            public Image GraphicImage = null;
            public UISpritePreset Preset = null;

            public IEnumerator ChangeSprite(string state)
            {
                UISpritePreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
                if (Preset.IsValid(setting.ExecutionState))
                {
                    if (!setting.IsEnabled) yield break;

                    yield return new WaitForSeconds(setting.DelayTime);
                    GraphicImage.sprite = setting.TargetSprite;
                }
            }
        }
    }
}
