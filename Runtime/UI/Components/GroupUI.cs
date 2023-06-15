using System;
using UnityEngine;

namespace Broccollie.UI
{
    [DefaultExecutionOrder(-110)]
    public class GroupUI : MonoBehaviour
    {
        #region Variable Field
        [Header("Group")]
        [SerializeField] private UIStates _triggerState = UIStates.Click;
        [SerializeField] private UIStates _triggeredState = UIStates.Default;
        [SerializeField] private BaseUI[] _baselineUIs = null;

        #endregion

        private void OnEnable()
        {
            switch (_triggerState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnShow += UnselectOthers;
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnHide += UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnInteractive += UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnNonInteractive += UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnDefault += UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnHover += UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnPress += UnselectOthers;
                    break;

                case UIStates.Click:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnClick += UnselectOthers;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (_triggerState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnShow -= UnselectOthers;
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnHide -= UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnInteractive -= UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnNonInteractive -= UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnDefault -= UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnHover -= UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnPress -= UnselectOthers;
                    break;

                case UIStates.Click:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnClick -= UnselectOthers;
                    break;
            }
        }

        #region Subscribers
        private void UnselectOthers(BaseUI sender, EventArgs args)
        {
            switch (_triggeredState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetVisible(true, false, false);
                    }
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetVisible(false, false, false);
                    }
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetInteractive(true, false, false);
                    }
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetInteractive(false, false, false);
                    }
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IDefaultUI>(out IDefaultUI interaction))
                            interaction.Default(false, false);
                    }
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IHoverUI>(out IHoverUI interaction))
                            interaction.Hover(false, false);
                    }
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IPressUI>(out IPressUI interaction))
                            interaction.Press(null, false, false);
                    }
                    break;

                case UIStates.Click:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == sender || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IClickUI>(out IClickUI interaction))
                            interaction.Click(false, false);
                    }
                    break;
            }
        }

        #endregion
    }
}
