using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Helper;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIPositionFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private Transform _targetGroup = null;
        [SerializeField] private bool IsEnabled = true;
        [SerializeField] private UIPositionPreset Preset = null;

        private UIPositionPreset.Setting _selectedSetting;
        #endregion

        #region Public Functions
        public void SetFeature(UIState state, out float duration)
        {
            duration = 0f;
            if (state == UIState.None) return;

            UIPositionPreset.Setting setting = Array.Find(Preset.Settings, x => x.State == state);
            if (setting.IsValid() && setting.IsEnabled)
            {
                duration = setting.Duration;
                _selectedSetting = setting;
            }
        }

        public IEnumerator Execute(UIState state)
        {
            if (!_selectedSetting.IsValid() && _selectedSetting.State != state)
            {
                yield break;
            }
            yield return _targetGroup.LerpPosition(_selectedSetting.TargetPoint.position, _selectedSetting.Duration);
        }

        #endregion

        #region Private Functions

        #endregion
    }

    public enum WaypointStyle { Linear, Donut }
}
