using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UISpriteFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;
        #endregion

        #region ColorChanger Functions
        public void Change(ButtonState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeSprite(this, state);
            }
        }

        #endregion

        [Serializable]
        public class Element
        {
            #region Variabled Field
            public bool IsEnabled = true;
            public Image GraphicImage = null;
            public UISpritePreset Preset = null;

            private IEnumerator _spriteChangeAction = null;
            #endregion

            #region Public Functions
            public void ChangeSprite(MonoBehaviour mono, ButtonState state)
            {
                if (_spriteChangeAction != null)
                    mono.StopCoroutine(_spriteChangeAction);

                UISpritePreset.SpriteState? spriteState = Array.Find(Preset.SpriteStates, x => x.ExecutionState == state);
                if (spriteState == null)
                    spriteState = Array.Find(Preset.SpriteStates, x => x.ExecutionState == ButtonState.Default);

                if (spriteState != null)
                {
                    if (!spriteState.Value.IsEnabled) return;

                    _spriteChangeAction = ChangeSprite();
                    mono.StartCoroutine(_spriteChangeAction);
                }

                IEnumerator ChangeSprite()
                {
                    GraphicImage.sprite = spriteState.Value.TargetSprite;
                    yield return null;
                }
            }
            #endregion
        }
    }
}
