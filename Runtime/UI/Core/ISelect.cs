using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public interface ISelect : IPointerClickHandler
    {
        public event Action<UIEventArgs> OnSelect;

        public Task SelectAsync(bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
