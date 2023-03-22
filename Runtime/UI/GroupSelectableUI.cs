using UnityEngine;

namespace Broccollie.UI
{
    public class GroupSelectableUI : MonoBehaviour
    {
        #region Variable Field
        [Header("Group")]
        [SerializeField] private BaselineUI[] _baselineUIs = null;

        #endregion

        private void OnEnable()
        {
            for (int i = 0; i < _baselineUIs.Length; i++)
                _baselineUIs[i].OnSelect += UnselectOthers;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _baselineUIs.Length; i++)
                _baselineUIs[i].OnSelect -= UnselectOthers;
        }

        #region Subscribers
        private void UnselectOthers(BaselineUI ui)
        {
            for (int i = 0; i < _baselineUIs.Length; i++)
            {
                if (_baselineUIs[i] == ui || !_baselineUIs[i].IsInteractive) continue;

                if (_baselineUIs[i].TryGetComponent<IDefaultUI>(out IDefaultUI defaultUI))
                    defaultUI.Default(false, false);
            }
        }

        #endregion
    }
}
