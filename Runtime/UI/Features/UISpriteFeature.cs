using System;
using System.Collections;
using System.Collections.Generic;
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
        protected override List<IEnumerator> GetFeatures(UIStates state)
        {
            List<IEnumerator> features = new List<IEnumerator>();
            for (int i = 0; i < _elements.Length; i++)
            {
                if (!_elements[i].IsEnabled) continue;

                UISpritePreset.SpriteSetting setting = Array.Find(_elements[i].Preset.Settings, x => x.ExecutionState == state);
                if (!setting.IsEnabled) continue;

                features.Add(SpriteSwap(_elements[i].Graphic, setting));
            }
            return features;
        }

        #endregion

        #region Private Functions
        public IEnumerator SpriteSwap(Image image, UISpritePreset.SpriteSetting setting)
        {
            if (setting.Delay > 0)
                yield return new WaitForSeconds(setting.Delay);
            image.sprite = setting.Sprite;
        }

        #endregion

        [Serializable]
        public struct Element
        {
            public bool IsEnabled;
            public Image Graphic;
            public UISpritePreset Preset;
        }
    }
}
