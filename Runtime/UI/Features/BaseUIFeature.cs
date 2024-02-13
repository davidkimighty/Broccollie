using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.UI
{
    public abstract class BaseUIFeature : MonoBehaviour
    {
        [SerializeField] protected bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
        }

        public abstract List<Task> GetFeatures(UIStates state, bool instantChange, bool playAudio, CancellationToken ct);
    }
}
