#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Broccollie.UI.Editor
{
    //[CustomEditor(typeof(ButtonUI))]
    public class ButtonUIEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _uxml = default;

        public override VisualElement CreateInspectorGUI()
        {
            ButtonUI buttonUI = (ButtonUI)target;

            VisualElement root = new VisualElement();
            _uxml.CloneTree(root);

            Button addColorFeatureButton = root.Q<Button>("button-add-colorfeature");
            Button removeColorFeatureButton = root.Q<Button>("button-remove-colorfeature");
            addColorFeatureButton.SetEnabled(!buttonUI.CheckComponentEditor<UIColorFeature>());
            removeColorFeatureButton.SetEnabled(buttonUI.CheckComponentEditor<UIColorFeature>());
            addColorFeatureButton.clicked += () => AddFeature<UIColorFeature>(buttonUI, addColorFeatureButton, removeColorFeatureButton);

            return root;
        }

        #region Subscribers
        private void AddFeature<T>(ButtonUI button, Button addButton, Button removeButton) where T : UIBaseFeature
        {
            addButton.visible = false;
            removeButton.visible = true;
            button.AddFeatureComponentEditor<T>();
            button.HideFeatureComponentsEditor();
        }

        private void RemoveFeature<T>(ButtonUI button, Button addButton, Button removeButton) where T : UIBaseFeature
        {

        }
        #endregion
    }
}
#endif