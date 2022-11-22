using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Variable Field
        public List<UIScrollElement> _scrollElements = new List<UIScrollElement>();

        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
        [SerializeField] private bool _scrollWhenRelease = true;

        private float[] _anchorPoints = null;
        private float _anchorPoint = 0f;
        private float _subdivisionDist = 0f;

        private float _scrollbarValue = 0f;
        private int _childCount = 0;
        private bool _dragging = false;

        #endregion

        private void Start()
        {
            _childCount = _scrollElements.Count;
            _anchorPoints = new float[_childCount];
            _subdivisionDist = 1f / (_childCount - 1);
            
            for (int i = 0; i < _childCount; i++)
                _anchorPoints[i] = _subdivisionDist * i;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }

        #region Public Functions
        
        #endregion

        private void Update()
        {
            if (_dragging || (_scrollWhenRelease && GetScrollSpeed() > _scrollStopSpeed))
            {
                FireEvents();

                _scrollbarValue = _scrollbar.value;
                NextAnchorPoint();
            }
            else if (Snapping())
            {
                FireEvents();

                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _anchorPoint, _transitionSpeed * Time.deltaTime);
            }
        }

        #region Private Functions
        private void NextAnchorPoint()
        {
            if (_scrollbarValue < 0)
                _anchorPoint = 0;
            else
            {
                for (int i = 0; i < _childCount; i++)
                {
                    if (_scrollbarValue < _anchorPoints[i] + (_subdivisionDist / 2) && _scrollbarValue > _anchorPoints[i] - (_subdivisionDist / 2))
                    {
                        _anchorPoint = _anchorPoints[i];
                        break;
                    }
                    if (i == _childCount - 1)
                        _anchorPoint = _anchorPoints[i];
                }
            }
        }

        private void FireEvents()
        {
            for (int i = 0; i < _anchorPoints.Length; i++)
            {
                if (_anchorPoints[i] == _anchorPoint)
                    _scrollElements[i].Focus();
                else
                    _scrollElements[i].Unfocus();
            }
        }

        private float GetScrollSpeed()
        {
            return Mathf.Abs(_scrollbarValue - _scrollbar.value) / Time.deltaTime;
        }

        private bool Snapping()
        {
            return Mathf.Abs(_scrollbar.value - _anchorPoint) > 0.01f;
        }

        #endregion
    }
}
