using System;
using System.Threading.Tasks;

namespace Broccollie.UI
{
    public interface IInteractive
    {
        public event Action<UIEventArgs> OnInteractive;
        public event Action<UIEventArgs> OnNonInteractive;

        public Task SetInteractiveAsync(bool state, bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
