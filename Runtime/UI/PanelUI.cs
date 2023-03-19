using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public class PanelUI : BaselineUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private GameObject _panel = null;

        [Header("Features")]
        [SerializeField] private UIColorFeature _colorFeature = null;
        [SerializeField] private UISpriteFeature _spriteFeature = null;
        [SerializeField] private UITransformFeature _transformFeature = null;
        [SerializeField] private UIAudioFeature _audioFeature = null;

        private Task _featureTasks = null;

        #endregion

        #region Public Functions
        public override void SetActive(bool state)
        {
            if (state)
            {
                _panel.SetActive(true);
                RaiseOnShow();
                Task.Run(() => RaiseOnShowAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Show, true, () =>
                {
                    _isInteractive = true;
                    _featureTasks = ExecuteFeaturesAsync(UIStates.Default);
                });
            }
            else
            {
                _isInteractive = false;
                RaiseOnHide();
                Task.Run(() => RaiseOnHideAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Hide, true, () =>
                {
                    _panel.SetActive(false);
                });
            }
        }

        public override void SetInteractive(bool state)
        {
            if (state)
            {
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                RaiseOnInteractive();
                Task.Run(() => RaiseOnInteractiveAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.Interactive, true, () =>
                {
                    _isInteractive = true;
                });
            }
            else
            {
                if (!_panel.activeSelf)
                    _panel.SetActive(true);

                _isInteractive = false;
                RaiseOnInteractive();
                Task.Run(() => RaiseOnInteractiveAsync());

                _featureTasks = ExecuteFeaturesAsync(UIStates.NonInteractive);
            }
        }

        #endregion

        #region Private Functions
        private async Task ExecuteFeaturesAsync(UIStates state, bool playAudio = true, Action done = null)
        {
            List<Task> featureTasks = new List<Task>();
            if (_colorFeature != null)
                featureTasks.Add(_colorFeature.ExecuteFeaturesAsync(state));

            if (_spriteFeature != null)
                featureTasks.Add(_spriteFeature.ExecuteFeaturesAsync(state));

            if (_transformFeature != null)
                featureTasks.Add(_transformFeature.ExecuteFeaturesAsync(state));

            if (_audioFeature != null)
                featureTasks.Add(_audioFeature.ExecuteFeaturesAsync(state));

            await Task.WhenAll(featureTasks);
            done?.Invoke();
        }

        #endregion
    }
}
