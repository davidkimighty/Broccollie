using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public interface IHover : IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action<UIEventArgs> OnHover;

        public Task HoverAsync(bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
