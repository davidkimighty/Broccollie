using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : BaseUI
    {
        #region Variable Field
        public event Action<UIEventArgs> OnDefault = null;
        public event Action<UIEventArgs> OnHovered = null;
        public event Action<UIEventArgs> OnPressed = null;
        public event Action<UIEventArgs> OnSelected = null;
        public event Action<UIEventArgs> OnDisabled = null;

        [Header("Button")]
        [SerializeField] private ButtonType type = ButtonType.Button;
        [SerializeField] private UIColorChanger colorChanger = null;
        #endregion

        private void Start()
        {
            hovering = pressed = selected = false;
            if (interactable)
                DefaultButton(true);
            else
                DisabledButton(true);
        }

        #region Button Functions
        /// <summary>
        /// Change button state with event invocation.
        /// </summary>
        public void ChangeState(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Default: InvokeExitAction(); break;
                case ButtonState.Hovered: InvokeEnterAction(); break;
                case ButtonState.Pressed: InvokeDownAction(); break;
                case ButtonState.Selected: InvokeClickAction(); break;
                case ButtonState.Disabled: InvokeDisableAction(); break;
            }
        }

        /// <summary>
        /// Change button state only visually.
        /// </summary>
        public void ChangeStateQuietly(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Default: DefaultButton(); break;
                case ButtonState.Hovered: HoveredButton(); break;
                case ButtonState.Pressed: PressedButton(); break;
                case ButtonState.Selected: SelectedButton(); break;
                case ButtonState.Disabled: DisabledButton(); break;
            }
        }
        #endregion

        #region Button Interaction Publishers
        protected override sealed void InvokeEnterAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!interactable) return;

            hovering = true;
            if (selected) return;

            HoveredButton();

            OnHovered?.Invoke(new UIEventArgs(this));
            //Debug.Log("[UIButton] Invoke Hovered");
        }

        protected override sealed void InvokeExitAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!interactable) return;

            hovering = false;
            if (selected) return;

            DefaultButton();

            OnDefault?.Invoke(new UIEventArgs(this));
            //Debug.Log("[UIButton] Invoke Default");
        }

        protected override sealed void InvokeDownAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!interactable) return;

            PressedButton();

            OnPressed?.Invoke(new UIEventArgs(this));
            //Debug.Log("[UIButton] Invoke Pressed");
        }

        protected override sealed void InvokeUpAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!interactable) return;

            pressed = false;
            CancelInteraction();

            void CancelInteraction()
            {
                if (!selected && !hovering)
                {
                    ChangeColors(ButtonState.Default);
                }
                else if (selected && !hovering)
                {
                    ChangeColors(ButtonState.Selected);
                }
            }
        }

        protected override sealed void InvokeClickAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!interactable) return;

            SelectedButton();

            if (selected)
            {
                OnSelected?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Selected");
            }
            else if (hovering)
            {
                OnHovered?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Hovered");
            }
            else
            {
                OnDefault?.Invoke(new UIEventArgs(this));
                //Debug.Log("[UIButton] Invoke Default");
            }
        }

        private void InvokeDisableAction()
        {
            DisabledButton();

            OnDisabled?.Invoke(new UIEventArgs(this));
            //Debug.Log("[UIButton] Invoke Disabled");
        }
        #endregion

        #region Button Behaviors
        private void DefaultButton(bool instantChange = false)
        {
            hovering = false;
            ChangeColors(ButtonState.Default, instantChange);
        }

        private void HoveredButton(bool instantChange = false)
        {
            hovering = true;
            ChangeColors(ButtonState.Hovered, instantChange);
        }

        private void PressedButton(bool instantChange = false)
        {
            pressed = true;
            ChangeColors(ButtonState.Pressed, instantChange);
        }

        private void SelectedButton(bool instantChange = false)
        {
            selected = type switch
            {
                ButtonType.Radio => true,
                ButtonType.Checkbox => !selected,
                _ => false
            };

            if (selected)
            {
                ChangeColors(ButtonState.Selected, instantChange);
            }
            else if (hovering)
            {
                ChangeColors(ButtonState.Hovered, instantChange);
            }
            else
            {
                ChangeColors(ButtonState.Default, instantChange);
            }
        }

        private void DisabledButton(bool instantChange = false)
        {
            interactable = false;
            ChangeColors(ButtonState.Disabled, instantChange);
        }
        #endregion

        #region Button Features
        private void ChangeColors(ButtonState state, bool instantChange = false)
        {
            if (colorChanger == null) return;

            if (instantChange)
                colorChanger.ChangeInstantly(state);
            else
                colorChanger.ChangeGradually(state);
        }

        private void ChangeSprites(ButtonState state)
        {

        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    public enum ButtonState { Default, Hovered, Pressed, Selected, Disabled }
}
