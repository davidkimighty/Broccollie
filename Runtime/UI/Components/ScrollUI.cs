using System;
using System.Collections;
using System.Collections.Generic;
using Broccollie.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Broccollie.UI
{
    public class ScrollUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        #region Variable Field
        [SerializeField] private List<BaseUI> _scrollElements = new List<BaseUI>();
        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
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

        #endregion

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
            _knobClicked = true;
        }

        #endregion

        #region Public Functions
        public void AddScrollElement(BaseUI element)
        {
            if (_scrollElements == null)
                _scrollElements = new List<BaseUI>();
            _scrollElements.Add(element);
        }

        public void SelectPageWithIndex(int index)
        {
            _anchorPoint = _anchorPoints[index];
            if (_scrollElements[index].TryGetComponent<IClickUI>(out IClickUI element))
                element.Click();

            if (_useKnob)
            {
                if (_knobs[index].TryGetComponent<IClickUI>(out IClickUI knob))
                    knob.Click();
            }
        }

        public int GetPageCount() => _scrollElements.Count;

        #endregion

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _dragging = true;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _dragging = false;
        }

        private void Update()
        {
            if (_knobClicked)
            {
                if (!Snapping(_targetAnchorPoint))
                    _knobClicked = false;

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
                if (_knobClicked)
                    _knobClicked = false;
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

        // Remove this function in the future. Separate the visual logics.
        private void UpdateUI()
        {
            for (int i = 0; i < _anchorPoints.Length; i++)
            {
                if (_anchorPoints[i] == _anchorPoint)
                {
                    if (_scrollElements[i].TryGetComponent<IClickUI>(out IClickUI element))
                        element.Click(false, true);

                    if (_useKnob && _knobs[i].TryGetComponent<IClickUI>(out IClickUI knob))
                        knob.Click(false, false);
                }
                else
                {
                    if (_scrollElements[i].TryGetComponent<IDefaultUI>(out IDefaultUI element))
                        element.Default(false, true);

                    if (_useKnob && _knobs[i].TryGetComponent<IDefaultUI>(out IDefaultUI knob))
                        knob.Default(false, false);
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
