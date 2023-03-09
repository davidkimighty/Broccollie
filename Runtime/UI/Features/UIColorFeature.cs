using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIColorFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        #endregion

        #region Public Functions
        public override async Task ExecuteAsync(string state, CancellationToken cancellationToken, Action done = null)
        {
            if (!_isEnabled) return;

            List<Task> executions = new List<Task>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;

                UIColorPreset.Setting setting = Array.Find(element.Preset.States, x => x.ExecutionState.ToString() == state);
                if (IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    executions.Add(element.ChangeColor(state, setting, cancellationToken));
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
            public MaskableGraphic Graphic = null;
            public UIColorPreset Preset = null;

            public async Task ChangeColor(string state, UIColorPreset.Setting setting, CancellationToken cancellationToken)
            {
                await Graphic.ChangeColorGraduallyAsync(setting.TargetColor, setting.Duration, cancellationToken, setting.Curve);
            }

            public void ChangeColorInstant(string state, UIColorPreset.Setting setting)
            {
                Graphic.color = setting.TargetColor;
            }
        }
    }
}
