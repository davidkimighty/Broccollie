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

        #region Public Functions
        public void Change(InteractionState state)
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

            #region Features
            public void ChangeSprite(MonoBehaviour mono, InteractionState state)
            {
                if (_spriteChangeAction != null)
                    mono.StopCoroutine(_spriteChangeAction);

                UISpritePreset.SpriteState spriteState = Array.Find(Preset.SpriteStates, x => x.ExecutionState == state);
                if (!spriteState.IsValid())
                    spriteState = Array.Find(Preset.SpriteStates, x => x.ExecutionState == InteractionState.Default);

                if (spriteState.IsValid())
                {
                    if (!spriteState.IsEnabled) return;
                    _spriteChangeAction = ApplySprite();
                    mono.StartCoroutine(_spriteChangeAction);
                }

                IEnumerator ApplySprite()
                {
                    GraphicImage.sprite = spriteState.TargetSprite;
                    yield return null;
                }
            }
            #endregion
        }
    }
}
