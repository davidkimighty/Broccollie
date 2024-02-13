using System;
using System.Threading.Tasks;

namespace Broccollie.UI
{
    public interface IDefault
    {
        public event Action<UIEventArgs> OnDefault;

        public Task DefaultAsync(bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
