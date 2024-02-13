using UnityEngine;

namespace Broccollie.UI
{
    public class GroupUI : MonoBehaviour
    {
        [SerializeField] private UIStates _triggerState = UIStates.Select;
        [SerializeField] private UIStates _triggeredState = UIStates.Default;
        [SerializeField] private BaseUIElement[] _elements = null;

        private void OnEnable()
        {
            switch (_triggerState)
            {
                case UIStates.Active:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IActive>(out var active))
                            active.OnActive += UnselectOthers;
                    break;

                case UIStates.InActive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IActive>(out var active))
                            active.OnInActive += UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IInteractive>(out var interactive))
                            interactive.OnInteractive += UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IInteractive>(out var interactive))
                            interactive.OnNonInteractive += UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IDefault>(out var deFault))
                            deFault.OnDefault += UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IHover>(out var hover))
                            hover.OnHover += UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IPress>(out var press))
                            press.OnPress += UnselectOthers;
                    break;

                case UIStates.Select:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<ISelect>(out var select))
                            select.OnSelect += UnselectOthers;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (_triggerState)
            {
                case UIStates.Active:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IActive>(out var active))
                            active.OnActive -= UnselectOthers;
                    break;

                case UIStates.InActive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IActive>(out var active))
                            active.OnInActive -= UnselectOthers;
                    break;

                case UIStates.Interactive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IInteractive>(out var interactive))
                            interactive.OnInteractive -= UnselectOthers;
                    break;

                case UIStates.NonInteractive:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IInteractive>(out var interactive))
                            interactive.OnNonInteractive -= UnselectOthers;
                    break;

                case UIStates.Default:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IDefault>(out var deFault))
                            deFault.OnDefault -= UnselectOthers;
                    break;

                case UIStates.Hover:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IHover>(out var hover))
                            hover.OnHover -= UnselectOthers;
                    break;

                case UIStates.Press:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<IPress>(out var press))
                            press.OnPress -= UnselectOthers;
                    break;

                case UIStates.Select:
                    for (int i = 0; i < _elements.Length; i++)
                        if (_elements[i].TryGetComponent<ISelect>(out var select))
                            select.OnSelect -= UnselectOthers;
                    break;
            }
        }

        #region Subscribers
        private async void UnselectOthers(UIEventArgs args)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i] == args.Sender || !_elements[i].IsRaycastInteractive) continue;

                switch (_triggeredState)
                {
                    case UIStates.Active:
                        if (_elements[i].TryGetComponent<IActive>(out var active))
                            await active.SetActiveAsync(true, false, false, false);
                        break;

                    case UIStates.InActive:
                        if (_elements[i].TryGetComponent<IActive>(out var inactive))
                            await inactive.SetActiveAsync(false, false, false, false);
                        break;

                    case UIStates.Interactive:
                        if (_elements[i].TryGetComponent<IInteractive>(out var interactive))
                            await interactive.SetInteractiveAsync(true, false, false, false);
                        break;

                    case UIStates.NonInteractive:
                        if (_elements[i].TryGetComponent<IInteractive>(out var noninteractive))
                            await noninteractive.SetInteractiveAsync(false, false, false, false);
                        break;

                    case UIStates.Default:
                        if (_elements[i].TryGetComponent<IDefault>(out var deFault))
                            await deFault.DefaultAsync(false, false, false);
                        break;

                    case UIStates.Hover:
                        if (_elements[i].TryGetComponent<IHover>(out var hover))
                            await hover.HoverAsync(false, false, false);
                        break;

                    case UIStates.Press:
                        if (_elements[i].TryGetComponent<IPress>(out var press))
                            await press.PressAsync(false, false, false);
                        break;

                    case UIStates.Select:
                        if (_elements[i].TryGetComponent<ISelect>(out var select))
                            await select.SelectAsync(false, false, false);
                        break;
                }
            }
        }

        #endregion
    }
}
