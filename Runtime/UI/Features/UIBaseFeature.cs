using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    public class UIBaseFeature : MonoBehaviour
    {
        #region Variable Field
        [Header("Base Feature")]
        [SerializeField] private bool _isEnabled = true;

        private List<IEnumerator> _featureCoroutines = new List<IEnumerator>();

        #endregion

        #region Public Functions
        public async Task ExecuteFeaturesAsync(UIStates state)
        {
            if (!_isEnabled) return;

            if (_featureCoroutines.Count > 0)
            {
                foreach (IEnumerator coroutine in _featureCoroutines)
                    StopCoroutine(coroutine);
                _featureCoroutines.Clear();
            }

            _featureCoroutines.AddRange(GetFeatures(state));

            if (gameObject.activeInHierarchy)
            {
                foreach (IEnumerator coroutine in _featureCoroutines)
                    StartCoroutine(coroutine);
            }
            await Task.Yield();
        }

        #endregion

        #region Protected Functions
        protected virtual List<IEnumerator> GetFeatures(UIStates state)
        {
            return new List<IEnumerator>();
        }

        #endregion
    }
}
