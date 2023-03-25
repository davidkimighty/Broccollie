using UnityEngine;

namespace Broccollie.UI
{
    public class GroupInteractableUI : MonoBehaviour
    {
        #region Variable Field
        [Header("Group")]
        [SerializeField] private UIStates _triggerState = UIStates.Select;
        [SerializeField] private UIStates _triggeredState = UIStates.Default;
        [SerializeField] private BaselineUI[] _baselineUIs = null;

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

                case UIStates.Select:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnSelect += UnselectOthers;
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

                case UIStates.Select:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                        _baselineUIs[i].OnSelect -= UnselectOthers;
                    break;
            }
        }

        #region Subscribers
        private void UnselectOthers(BaselineUI ui)
        {
            switch (_triggeredState)
            {
                case UIStates.Show:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetActive(true, false, false);
                    }
                    break;

                case UIStates.Hide:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetActive(false, false, false);
                    }
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetInteractive(true, false, false);
                    }
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        _baselineUIs[i].SetInteractive(false, false, false);
                    }
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IDefaultUI>(out IDefaultUI interaction))
                            interaction.Default(false, false);
                    }
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IHoverUI>(out IHoverUI interaction))
                            interaction.Hover(false, false);
                    }
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<IPressUI>(out IPressUI interaction))
                            interaction.Press(false, false);
                    }
                    break;

                case UIStates.Select:
                    for (int i = 0; i < _baselineUIs.Length; i++)
                    {
                        if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;
                        if (_baselineUIs[i].TryGetComponent<ISelectUI>(out ISelectUI interaction))
                            interaction.Select(false, false);
                    }
                    break;
            }
        }

        #endregion
    }
}
