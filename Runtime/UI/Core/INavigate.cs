using System;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public interface INavigate : IMoveHandler, ISubmitHandler
    {
        public event Action<UIEventArgs, MoveDirection> OnNavigateMove;
        public event Action<UIEventArgs> OnNavigateSubmit;

        public void Navigate(MoveDirection direction, bool invokeEvent = true);

        public void Navigate(AxisEventData eventData, bool invokeEvent = true);

        public Task SubmitAsync(bool instantChange = false, bool playAudio = true, bool invokeEvent = true);
    }
}
