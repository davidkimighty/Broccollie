using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Variable Field
        [SerializeField] private List<UIScrollElement> _scrollElements = new List<UIScrollElement>();
        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
        [SerializeField] private bool _scrollWhenRelease = true;

        [Header("Knob")]
        [SerializeField] private bool _useKnob = true;
        [SerializeField] private UIKnob _knobPrefab = null;
        [SerializeField] private Transform _knobHolder = null;

        private float[] _anchorPoints = null;
        private float _anchorPoint = 0f;
        private float _subdivisionDist = 0f;

        private float _scrollbarValue = 0f;
        private int _childCount = 0;
        private bool _dragging = false;

        private UIKnob[] _knobs = null;
        private bool _knobSelected = false;
        private float _targetAnchorPoint = 0f;

        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            _anchorPoint = _anchorPoints[0];
            _scrollElements[0].Focus(false);

            if (_useKnob)
                _knobs[0].ChangeState(InteractionState.Selected);
        }

        #region Subscribers
        private void SelectKnob(int index)
        {
            _targetAnchorPoint = _anchorPoints[index];
            _knobSelected = true;
        }

        #endregion

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }

        #region Public Functions
        public void AddScrollElement(UIScrollElement element)
        {
            _scrollElements.Add(element);
            Initialize();
        }

        #endregion

        private void Update()
        {
            if (_knobSelected)
            {
                if (!Snapping(_targetAnchorPoint))
                    _knobSelected = false;

                UpdateUI();
                NextAnchorPoint();
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetAnchorPoint, _transitionSpeed * Time.deltaTime);
            }
            else if (_dragging || (_scrollWhenRelease && GetScrollSpeed() > _scrollStopSpeed))
            {
                UpdateUI();
                NextAnchorPoint();
            }
            else if (Snapping(_anchorPoint))
            {
                UpdateUI();
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _anchorPoint, _transitionSpeed * Time.deltaTime);
            }
            else
            {
                if (_knobSelected)
                    _knobSelected = false;
            }
        }

        #region Private Functions
        private void Initialize()
        {
            _childCount = _scrollElements.Count;
            if (_childCount == 0) return;

            _anchorPoints = new float[_childCount];
            _subdivisionDist = 1f / (_childCount - 1);

            for (int i = 0; i < _childCount; i++)
                _anchorPoints[i] = _subdivisionDist * i;

            if (_useKnob)
            {
                if (_knobHolder.childCount > 0)
                {
                    for (int i = _knobHolder.childCount - 1; i >= 0; i--)
                        Destroy(_knobHolder.GetChild(i).gameObject);
                    _knobs = null;
                }

                List<UIKnob> knobsTemp = new List<UIKnob>();
                for (int i = 0; i < _childCount; i++)
                {
                    UIKnob knob = Instantiate<UIKnob>(_knobPrefab, _knobHolder);
                    knobsTemp.Add(knob);

                    int index = i;
                    knob.OnSelected += (eventArgs) => SelectKnob(index);
                }
                _knobs = knobsTemp.ToArray();
            }
        }

        private void NextAnchorPoint()
        {
            _scrollbarValue = _scrollbar.value;
            if (_scrollbarValue < 0)
                _anchorPoint = 0;
            else
            {
                for (int i = 0; i < _childCount; i++)
                {
                    if (_scrollbarValue < _anchorPoints[i] + (_subdivisionDist / 2) &&
                        _scrollbarValue > _anchorPoints[i] - (_subdivisionDist / 2))
                    {
                        _anchorPoint = _anchorPoints[i];
                        break;
                    }
                    if (i == _childCount - 1)
                        _anchorPoint = _anchorPoints[i];
                }
            }
        }

        private void UpdateUI()
        {
            for (int i = 0; i < _anchorPoints.Length; i++)
            {
                if (_anchorPoints[i] == _anchorPoint)
                {
                    _scrollElements[i].Focus();
                    _knobs[i].ChangeState(InteractionState.Selected, false, false, false);
                }
                else
                {
                    _scrollElements[i].Unfocus();
                    _knobs[i].ChangeState(InteractionState.Default, false, false, false);
                }
            }
        }

        private float GetScrollSpeed()
        {
            return Mathf.Abs(_scrollbarValue - _scrollbar.value) / Time.deltaTime;
        }

        private bool Snapping(float anchorPoint)
        {
            return Mathf.Abs(_scrollbar.value - anchorPoint) > 0.01f;
        }

        #endregion
    }
}
