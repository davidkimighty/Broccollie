using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDragFeature : BaseUIFeature
    {
        #region Variable Field
        [SerializeField] private Transform _dragTarget = null;
        [SerializeField] private Canvas _dragArea = null;
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private UIDragPreset _preset = null;

        private bool _dragging = false;
        private Vector3 _lastPosition = Vector2.zero;
        private Task _dragTask = null;
        #endregion

        #region Public Functions
        public override async Task ExecuteAsync(PointerEventData eventData = null, Action done = null)
        {
            switch (_dragArea.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    _lastPosition = eventData.position;
                    break;

                case RenderMode.ScreenSpaceCamera:
                    Vector3 point = new Vector3(eventData.position.x, eventData.position.y, _dragArea.planeDistance);
                    Vector3 screen2World = Camera.main.ScreenToWorldPoint(point);
                    screen2World.z = _dragArea.planeDistance;
                    _lastPosition = screen2World;
                    break;
            }

            if (_dragTask == null)
                _dragTask = UpdateDraggablePosition();

            await Task.Yield();
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

        private async Task UpdateDraggablePosition()
        {
            while (true)
            {
                if (_dragging)
                {
                    _dragTarget.position = Vector3.Lerp(_dragTarget.position, _lastPosition, _preset.DragSpeed);
                    await Task.Yield();
                }
                else
                {
                    await Task.Yield();
                }
            }
        }
    }
}
