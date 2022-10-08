using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIPanelGroup : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private List<BaseUIPanel> _panels = null;
        #endregion

        private void OnEnable()
        {
            foreach (BaseUIPanel panel in _panels)
                panel.OnShow += HideOthers;
        }

        private void OnDisable()
        {
            foreach (BaseUIPanel panel in _panels)
                panel.OnShow -= HideOthers;
        }

        #region Subscribers
        private void HideOthers(InteractableEventArgs args)
        {
            if (!args.IsValid()) return;

            foreach (BaseUIPanel panel in _panels)
            {
                if (panel == args.Sender) continue;
                panel.SetPanelVisible(false, false);
            }
        }
        #endregion
    }
}
