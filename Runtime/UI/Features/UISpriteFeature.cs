using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.UI
{
    public class UISpriteFeature : UIBaseFeature
    {
        #region Variable Field
        [Header("Sprite Feature")]
        [SerializeField] private Element[] _elements = null;

        #endregion

        #region Override Functions
        protected override List<Task> GetFeatures(UIStates state, CancellationToken ct)
        {
            List<Task> features = new List<Task>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UISpritePreset.SpriteSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                features.Add(SpriteSwapAsync(_elements[i].Graphic, setting, ct));
            }
            return features;
        }

        protected override List<Action> GetFeaturesInstant(UIStates state)
        {
            List<Action> features = new List<Action>();
            if (_elements == null) return features;

            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UISpritePreset.SpriteSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (setting == null || !setting.IsEnabled) continue;

                int index = i;
                features.Add(() => SpriteSwapInstant(_elements[index].Graphic, setting));
            }
            return features;
        }

        #endregion

        #region Private Functions
        private async Task SpriteSwapAsync(Image image, UISpritePreset.SpriteSetting setting, CancellationToken ct)
        {
            if (setting.Delay > 0)
                await Task.Delay((int)(setting.Delay * 1000f), ct);
            image.sprite = setting.Sprite;
        }

        private void SpriteSwapInstant(Image image, UISpritePreset.SpriteSetting setting)
        {
            image.sprite = setting.Sprite;
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled;
            public Image Graphic;
            public UISpritePreset Preset;
        }
    }
}
