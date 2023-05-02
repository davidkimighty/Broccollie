using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    public class UIBaseFeature : MonoBehaviour
    {
        #region Variable Field
        [Header("Base Feature")]
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private FeatureTypes _featureType = FeatureTypes.None;
        public FeatureTypes FeatureType
        {
            get => _featureType;
        }

        private List<Task> _featureTasks = new List<Task>();

        #endregion

        #region Public Functions
        public async Task ExecuteAsync(UIStates state, CancellationToken ct)
        {
            if (!_isEnabled) return;

            _featureTasks.AddRange(GetFeatures(state, ct));
            await Task.WhenAll(_featureTasks);
        }

        public void ExecuteInstant(UIStates state)
        {
            if (!_isEnabled) return;

            List<Action> instantFeatures = GetFeaturesInstant(state);
            foreach (Action feature in instantFeatures)
                feature?.Invoke();
        }

        #endregion

        #region Protected Functions
        protected virtual List<Task> GetFeatures(UIStates state, CancellationToken ct)
        {
            return new List<Task>();
        }

        protected virtual List<Action> GetFeaturesInstant(UIStates state)
        {
            return new List<Action>();
        }

        #endregion
    }

    public enum FeatureTypes { None, Audio, }
}
