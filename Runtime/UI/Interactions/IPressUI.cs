using UnityEngine.EventSystems;

namespace Broccollie.UI
{
    public interface IPressUI
    {
        void Press(PointerEventData eventData = null, bool playAudio = false, bool invokeEvent = true);
    }
}
