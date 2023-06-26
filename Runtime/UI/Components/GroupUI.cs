using System;
using UnityEngine;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-110)]
    public class GroupUI : MonoBehaviour
    {
        [Header("Group")]
        [SerializeField] private UIStates _triggerState = UIStates.Click;
        [SerializeField] private UIStates _triggeredState = UIStates.Default;
        [SerializeField] private BaseUI[] _elements = null;

        private void OnEnable()
        {
            switch (_triggerState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnShow += UnselectOthers;
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnHide += UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnInteractive += UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnNonInteractive += UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnDefault += UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnHover += UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnPress += UnselectOthers;
                    break;

                case UIStates.Click:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnClick += UnselectOthers;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (_triggerState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnShow -= UnselectOthers;
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnHide -= UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnInteractive -= UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnNonInteractive -= UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnDefault -= UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnHover -= UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnPress -= UnselectOthers;
                    break;

                case UIStates.Click:
                    for (int i = 0; i < _elements.Length; i++)
                        _elements[i].OnClick -= UnselectOthers;
                    break;
            }
        }

        #region Subscribers
        private void UnselectOthers(BaseUI sender, EventArgs args)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i] == sender || !_elements[i].IsInteractive) continue;
                _elements[i].ChangeState(_triggeredState.ToString(), false, false, false);
            }
        }

        #endregion
    }
}
