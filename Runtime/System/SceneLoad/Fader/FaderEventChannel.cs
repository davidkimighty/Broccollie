using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.System
{
    [CreateAssetMenu(fileName = "EventChannel_Fader", menuName = "Broccollie/EventChannels/Fader")]
    public class FaderEventChannel : ScriptableObject
    {
        public event Action<float> OnFade;
        public event Func<float, Task> OnFadeAsync;

        #region Publishers
        public void RequestFade(float alpha)
        {
            OnFade?.Invoke(alpha);
        }

        public async Task RequestFadeAsync(float alpha)
        {
            if (OnFadeAsync != null)
                await OnFadeAsync.Invoke(alpha);
        }

        #endregion
    }
}
