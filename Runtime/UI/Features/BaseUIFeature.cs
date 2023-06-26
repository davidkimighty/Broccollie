using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    public class BaseUIFeature : MonoBehaviour
    {
        [Header("Base Feature")]
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private FeatureTypes _featureType = FeatureTypes.None;
        public FeatureTypes FeatureType
        {
            get => _featureType;
        }

        private List<Task> _featureTasks = new List<Task>();

        #region Public Functions
        public async Task ExecuteAsync(string state, CancellationToken ct)
        {
            if (!_isEnabled) return;

            _featureTasks.AddRange(GetFeatures(state, ct));
            await Task.WhenAll(_featureTasks);
        }

        public void ExecuteInstant(string state)
        {
            if (!_isEnabled) return;

            List<Action> instantFeatures = GetFeaturesInstant(state);
            foreach (Action feature in instantFeatures)
                feature?.Invoke();
        }

        #endregion

        protected virtual List<Task> GetFeatures(string state, CancellationToken ct)
        {
            return new List<Task>();
        }

        protected virtual List<Action> GetFeaturesInstant(string state)
        {
            return new List<Action>();
        }
    }

    public enum FeatureTypes { None, Audio, }
}
