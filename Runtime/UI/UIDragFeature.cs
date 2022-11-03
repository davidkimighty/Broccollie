using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDragFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private Transform _dragTarget = null;
        [SerializeField] private Canvas _dragArea = null;
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private UIDragPreset _preset = null;

        private bool _dragging = false;
        private Vector2 _lastPosition = Vector2.zero;
        private IEnumerator _dragAction = null;
        #endregion

        #region Public Functions
        public void Drag(PointerEventData eventData)
        {
            _lastPosition = eventData.position;

            if (_dragAction == null)
            {
                _dragAction = UpdateDraggablePosition();
                StartCoroutine(_dragAction);
            }
        }

        public void SetBlocksRaycasts(bool state)
        {
            _canvasGroup.blocksRaycasts = state;
            _dragging = !state;
        }

        public void SetDragArea(Canvas canvas)
        {
            _dragArea = canvas;
        }
        #endregion

        private IEnumerator UpdateDraggablePosition()
        {
            while (true)
            {
                if (!_dragging)
                {
                    yield return null;
                }
                else
                {
                    _dragTarget.position = Vector2.Lerp(_dragTarget.position, _lastPosition, _preset.DragSpeed);
                    yield return null;
                }
            }
        }
    }
}
