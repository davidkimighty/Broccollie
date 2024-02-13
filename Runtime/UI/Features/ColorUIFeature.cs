using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Broccollie.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.UI
{
    [DisallowMultipleComponent]
    public class ColorUIFeature : BaseUIFeature
    {
        [SerializeField] private bool _useColorPalette = false;
        [SerializeField] private ColorPalette _colorPalette = null;
        [SerializeField] private Element[] _elements = null;

        #region Public Functions
        public override List<Task> GetFeatures(UIStates state, bool instantChange, bool playAudio, CancellationToken ct)
        {
            try
            {
                if (_elements == null) return default;

                List<Task> features = new List<Task>();
                for (int i = 0; i < _elements.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    if (!_elements[i].IsEnabled || _elements[i].Preset == null) continue;

                    ColorUIFeaturePreset.Setting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                    if (!setting.IsEnabled) continue;

                    if (instantChange)
                        features.Add(InstantColorChange(_elements[i].Graphic,
                            _useColorPalette ? _colorPalette.GetColor(setting.ColorPaletteKey) : setting.TargetColor, ct));
                    else
                        features.Add(_elements[i].Graphic.LerpColorAsync(
                            _useColorPalette ? _colorPalette.GetColor(setting.ColorPaletteKey) : setting.TargetColor, setting.Duration, ct, setting.Curve));
                }
                return features;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
        }

        #endregion

        private async Task InstantColorChange(MaskableGraphic graphic, Color color, CancellationToken ct)
        {
            graphic.color = color;
            await Task.Yield();
        }

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public MaskableGraphic Graphic;
            public ColorUIFeaturePreset Preset;
        }
    }
}
