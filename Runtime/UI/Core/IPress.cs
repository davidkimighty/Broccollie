using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public interface IPress : IPointerDownHandler, IPointerUpHandler
    {
        public event Action<UIEventArgs> OnPress;

        public Task PressAsync(bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
