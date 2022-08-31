using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public class UIButton : BaseUI
    {
        #region Variable Field
        [Header("Button")]
        [SerializeField] private ButtonType type = ButtonType.Button;
        [SerializeField] private UIColorChanger colorChanger = null;
        #endregion

        private void Start()
        {
            isHovering = isPressed = isSelected = false;

        }

        #region Button Features
        /// <summary>
        /// Change button state with event invocation.
        /// </summary>
        public void ChangeState(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Default: InvokeDefaultAction(); break;
                case ButtonState.Hovered: InvokeHoverInAction(); break;
                case ButtonState.Pressed: InvokePressAction(); break;
                case ButtonState.Selected: InvokeSelectAction(); break;
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

        #region Button Actions
        protected override sealed void InvokeDefaultAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!isInteractable) return;

            DefaultButton();
            // Sound feature
            base.InvokeDefaultAction(eventData, new UIEventArgs(this));
        }

        protected override sealed void InvokeHoverInAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!isInteractable) return;

            HoveredButton();
            // Sound feature
            base.InvokeHoverInAction(eventData, new UIEventArgs(this));
        }

        protected override sealed void InvokePressAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!isInteractable) return;

            PressedButton();
            // Sound feature
            base.InvokePressAction(eventData, new UIEventArgs(this));
        }

        protected override sealed void InvokeSelectAction(PointerEventData eventData = null, UIEventArgs args = null)
        {
            if (!isInteractable) return;

            SelectedButton();
            // Sound feature
            base.InvokeSelectAction(eventData, new UIEventArgs(this));
        }

        protected override void InvokeDisableAction(UIEventArgs args = null)
        {
            // Sound feature
            base.InvokeDisableAction(args);
        }
        #endregion

        #region Button Behaviors
        private void DefaultButton()
        {
            isHovering = isPressed;
            isPressed = false;

            switch (type)
            {
                case ButtonType.Button:
                    // Change visuals
                    break;

                case ButtonType.Radio:

                    break;

                case ButtonType.Checkbox:

                    break;
            }
        }

        private void HoveredButton()
        {
            isHovering = true;

        }

        private void PressedButton()
        {
            isPressed = true;

        }

        private void SelectedButton()
        {
            switch (type)
            {
                case ButtonType.Button:
                    // Change visuals
                    break;

                case ButtonType.Radio:
                    isSelected = true;

                    break;

                case ButtonType.Checkbox:
                    isSelected = !isSelected;
                    
                    break;
            }
        }

        private void DisabledButton()
        {
             isHovering = isPressed = isInteractable = false;

        }
        #endregion
    }

    public enum ButtonType { Button, Radio, Checkbox }
    public enum ButtonState { Default, Hovered, Pressed, Selected, Disabled }
}
