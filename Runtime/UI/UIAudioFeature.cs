using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Audio;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIAudioFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private AudioEventChannel _eventChannel = null;
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        #endregion

        #region Public Functions
        public override async Task ExecuteAsync(string state, CancellationTokenSource tokenSource, Action done = null)
        {
            if (!_isEnabled) return;

            List<Task> executions = new List<Task>();
            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;

                UIAudioPreset.Setting setting = Array.Find(element.Preset.States, x => x.ExecutionState.ToString() == state);
                if (IsValid(setting.ExecutionState) && setting.IsEnabled)
                {
                    executions.Add(element.PlayAudio(state, _eventChannel, setting));
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
            public UIAudioPreset Preset = null;

            public async Task PlayAudio(string state, AudioEventChannel eventChannel, UIAudioPreset.Setting setting)
            {
                eventChannel.RaisePlayAudioEvent(setting.AudioPreset);
                await Task.Yield();
            }
        }
    }
}
