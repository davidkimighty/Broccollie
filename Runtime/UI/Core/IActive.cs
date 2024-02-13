using System;
using System.Threading.Tasks;

namespace Broccollie.UI
{
    public interface IActive
    {
        public event Action<UIEventArgs> OnActive;
        public event Action<UIEventArgs> OnInActive;

        public Task SetActiveAsync(bool state, bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
