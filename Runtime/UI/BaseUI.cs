using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        #region Features
        protected abstract void SetActive(bool state);

        #endregion
    }

    public class UIEventArgs
    {
        public BaseUI Sender = null;

        public UIEventArgs() { }

        public UIEventArgs(BaseUI sender)
        {
            this.Sender = sender;
        }

        public bool IsValid()
        {
            return Sender != null;
        }
    }

    public enum UIState { None, Default, Interactive, NonInteractive, Show, Hide }
}
