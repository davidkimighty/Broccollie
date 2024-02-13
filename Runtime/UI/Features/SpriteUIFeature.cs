using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.UI
{
    public class SpriteUIFeature : BaseUIFeature
    {
        [SerializeField] private Element[] _elements = null;

        #region Public Functions
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

                    SpriteUIFeaturePreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                    if (!setting.IsEnabled) continue;

                    if (instantChange)
                        features.Add(SpriteSwapInstantAsync(_elements[i].Graphic, setting));
                    else
                        features.Add(SpriteSwapAsync(_elements[i].Graphic, setting, ct));
                }
                return features;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
        }

        #endregion

        private async Task SpriteSwapAsync(Image image, SpriteUIFeaturePreset.Setting setting, CancellationToken ct)
        {
            if (setting.Delay > 0)
                await Task.Delay((int)(setting.Delay * 1000f), ct);
            image.sprite = setting.Sprite;
        }

        private async Task SpriteSwapInstantAsync(Image image, SpriteUIFeaturePreset.Setting setting)
        {
            image.sprite = setting.Sprite;
            await Task.Yield();
        }

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public Image Graphic;
            public SpriteUIFeaturePreset Preset;
        }
    }
}
