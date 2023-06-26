using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Broccollie.UI
{
    public class ScrollUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public event Action OnBeginScroll = null;
        public event Action OnEndScroll = null;
        public event Action<BaseUI, int> OnFocusElement = null;
        public event Action<BaseUI> OnUnfocusElement = null;

        [SerializeField] private List<BaseUI> _scrollElements = new List<BaseUI>();
        public List<BaseUI> ScrollElements
        {
            get => _scrollElements;
        }
        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
        [SerializeField] private AnimationCurve _scrollCurve = null;
        [SerializeField] private bool _scrollWhenRelease = true;

        [Header("Knob")]
        [SerializeField] private bool _useKnob = true;
        [SerializeField] private BaseUI _knobPrefab = null;
        [SerializeField] private Transform _knobHolder = null;

        private float[] _anchorPoints = null;
        private float _anchorPoint = 0f;
        private float _subdivisionDist = 0f;

        private float _scrollbarValue = 0f;
        private int _childCount = 0;
        private bool _dragging = false;

        private BaseUI[] _knobs = null;
        public BaseUI[] Knobs
        {
            get => _knobs;
        }
        private bool _knobClicked = false;
        private float _targetAnchorPoint = 0f;
        private bool _dragStartFlag = false;
        private bool _scrollEndFlag = false;

        private void Awake()
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

                List<BaseUI> knobs = new List<BaseUI>();
                for (int i = 0; i < _childCount; i++)
                {
                    BaseUI knob = Instantiate<BaseUI>(_knobPrefab, _knobHolder);
                    knob.gameObject.name += " " + i.ToString();
                    knobs.Add(knob);

                    int index = i;
                    knob.OnClick += (eventArgs, sender) => ClickKnob(index);
                }
                _knobs = knobs.ToArray();
            }
        }

        #region Subscribers
        private void ClickKnob(int index)
        {
            _targetAnchorPoint = _anchorPoints[index];
            _knobClicked = _scrollEndFlag = true;
            OnBeginScroll?.Invoke();
        }

        #endregion

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _dragStartFlag = true;
            _dragging = true;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }

        #region Public Functions
        public void AddScrollElement(BaseUI element)
        {
            if (_scrollElements == null)
                _scrollElements = new List<BaseUI>();
            _scrollElements.Add(element);
        }

        public void SelectPageWithIndex(int index) => ClickKnob(index);

        public int GetPageCount() => _scrollElements.Count;

        #endregion

        private void Update()
        {
            if (_knobClicked)
            {
                if (!Snapping(_targetAnchorPoint))
                    _knobClicked = false;

                InvokeEvents();
                SetNextAnchorPoint();

                _scrollbar.value = Mathf.LerpUnclamped(_scrollbar.value, _targetAnchorPoint, _scrollCurve.Evaluate(_transitionSpeed * Time.deltaTime));
            }
            else if (_dragging || (_scrollWhenRelease && GetScrollSpeed() > _scrollStopSpeed))
            {
                if (_dragStartFlag)
                {
                    OnBeginScroll?.Invoke();
                    _dragStartFlag = false;
                    _scrollEndFlag = true;
                }
                InvokeEvents();
                SetNextAnchorPoint();
            }
            else if (Snapping(_anchorPoint))
            {
                InvokeEvents();
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _anchorPoint, _scrollCurve.Evaluate(_transitionSpeed * Time.deltaTime));
            }
            else
            {
                if (_scrollEndFlag)
                {
                    OnEndScroll?.Invoke();
                    _scrollEndFlag = false;
                }
                _knobClicked = false;
            }
        }

        private void InvokeEvents()
        {
            for (int i = 0; i < _anchorPoints.Length; i++)
            {
                if (_anchorPoints[i] == _anchorPoint)
                {
                    OnFocusElement?.Invoke(_scrollElements[i], i);

                    if (!_useKnob) continue;
                    OnFocusElement?.Invoke(_knobs[i], i);
                }
                else
                {
                    OnUnfocusElement?.Invoke(_scrollElements[i]);

                    if (!_useKnob) continue;
                    OnUnfocusElement?.Invoke(_knobs[i]);
                }
            }
        }

        private void SetNextAnchorPoint()
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

        private float GetScrollSpeed()
        {
            return Mathf.Abs(_scrollbarValue - _scrollbar.value) / Time.deltaTime;
        }

        private bool Snapping(float anchorPoint)
        {
            return Mathf.Abs(_scrollbar.value - anchorPoint) > 0.001f;
        }
    }
}
