using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        #endregion

        #region Public Functions
        public override async Task ExecuteAsync(string state, CancellationToken cancellationToken, Action done = null)
        {
            if (!_isEnabled) return;

            List<Task> executions = new List<Task>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;

                UISpritePreset.Setting setting = Array.Find(element.Preset.States, x => x.ExecutionState.ToString() == state);
                if (IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    executions.Add(element.ChangeSprite(state, setting, cancellationToken));
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
            public Image GraphicImage = null;
            public UISpritePreset Preset = null;

            public async Task ChangeSprite(string state, UISpritePreset.Setting setting, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay((int)setting.DelayTime);
                GraphicImage.sprite = setting.TargetSprite;
            }
        }
    }
}
