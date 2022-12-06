using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollieMollie.UI
{
    public class UIScroll : InteractableUI
    {
        #region Variable Field
        [SerializeField] private List<UIScrollElement> _scrollElements = new List<UIScrollElement>();
        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
        [SerializeField] private bool _scrollWhenRelease = true;
        [SerializeField] private UIInteractionState _focusState = UIInteractionState.Selected;
        [SerializeField] private UIState _unfocusState = UIState.Default;

        [Header("Knob")]
        [SerializeField] private bool _useKnob = true;
        [SerializeField] private UIButton _knobPrefab = null;
        [SerializeField] private Transform _knobHolder = null;

        private float[] _anchorPoints = null;
        private float _anchorPoint = 0f;
        private float _subdivisionDist = 0f;

        private float _scrollbarValue = 0f;
        private int _childCount = 0;

        private UIButton[] _knobs = null;
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
            _scrollElements[0].Focus(_focusState.ToString(), false);

            if (_useKnob)
                _knobs[0].ChangeInteractionState(_focusState);
        }

        #region Subscribers
        private void SelectKnob(int index)
        {
            _targetAnchorPoint = _anchorPoints[index];
            _knobSelected = true;
        }

        #endregion

        protected override void InvokeBeginDragAction(PointerEventData eventData = null)
        {
            _dragging = true;
        }

        protected override void InvokeEndDragAction(PointerEventData eventData = null)
        {
            _dragging = false;
        }

        #region Public Functions
        public void Initialize()
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

                List<UIButton> knobsTemp = new List<UIButton>();
                for (int i = 0; i < _childCount; i++)
                {
                    UIButton knob = Instantiate<UIButton>(_knobPrefab, _knobHolder);
                    knob.gameObject.name += " " + i.ToString();
                    knobsTemp.Add(knob);

                    int index = i;
                    knob.OnSelected += (eventArgs) => SelectKnob(index);
                }
                _knobs = knobsTemp.ToArray();
            }
        }

        public void AddScrollElement(UIScrollElement element)
        {
            if (_scrollElements == null)
                _scrollElements = new List<UIScrollElement>();
            _scrollElements.Add(element);
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
                    _scrollElements[i].Focus(_focusState.ToString());
                    _knobs[i].ChangeInteractionState(UIInteractionState.Selected, false, false);
                }
                else
                {
                    _scrollElements[i].Unfocus(_unfocusState.ToString(), false, true);
                    _knobs[i].ChangeState(UIState.Default, false, false);
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
