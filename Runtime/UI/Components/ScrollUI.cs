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
        public event Action<BaseUIElement, int> OnFocusElement = null;
        public event Action<BaseUIElement, int> OnUnfocusElement = null;

        [SerializeField] private string _preSetupElementsKey = "pre";
        [SerializeField] private List<BaseUIElement> _preSetupScrollElements = new();
        public List<BaseUIElement> ScrollElements
        {
            get => _preSetupScrollElements;
        }
        [SerializeField] private Transform _contentHolder = null;
        [SerializeField] private Scrollbar _scrollbar = null;
        [Range(1, 10)]
        [SerializeField] private int _transitionSpeed = 5;
        [Range(0.05f, 1)]
        [SerializeField] private float _scrollStopSpeed = 0.1f;
        [SerializeField] private AnimationCurve _scrollCurve = null;
        [SerializeField] private bool _scrollWhenRelease = true;

        [Header("Knob")]
        [SerializeField] private bool _useKnob = true;
        [SerializeField] private BaseUIElement _knobPrefab = null;
        [SerializeField] private Transform _knobHolder = null;

        private Dictionary<string, List<BaseUIElement>> _scrollElements = new();
        private Dictionary<string, float[]> _anchorPoints = new();
        private Dictionary<string, float> _subdivisionDists = new();
        private string _currentKey = "";
        private float _anchorPoint = 0f;

        private float _scrollbarValue = 0f;
        private bool _dragging = false;

        private BaseUIElement[] _knobs = null;
        public BaseUIElement[] Knobs
        {
            get => _knobs;
        }
        private bool _knobClicked = false;
        private float _targetAnchorPoint = 0f;
        private bool _dragStartFlag = false;
        private bool _scrollEndFlag = false;

        private void Awake()
        {
            int childCount = _preSetupScrollElements.Count;
            if (childCount == 0) return;

            _anchorPoints = new Dictionary<string, float[]> { { _preSetupElementsKey, new float[childCount] } };
            _subdivisionDists = new Dictionary<string, float> { { _preSetupElementsKey, 1f / (childCount - 1) } };

            for (int i = 0; i < childCount; i++)
                _anchorPoints[_preSetupElementsKey][i] = _subdivisionDists[_preSetupElementsKey] * i;

            _scrollElements = new Dictionary<string, List<BaseUIElement>>() { { _preSetupElementsKey, _preSetupScrollElements } };
            _currentKey = _preSetupElementsKey;

            if (_useKnob)
            {
                if (_knobHolder.childCount > 0)
                {
                    for (int i = _knobHolder.childCount - 1; i >= 0; i--)
                        Destroy(_knobHolder.GetChild(i).gameObject);
                    _knobs = null;
                }

                List<BaseUIElement> knobs = new();
                for (int i = 0; i < childCount; i++)
                {
                    BaseUIElement knob = Instantiate(_knobPrefab, _knobHolder);
                    knob.gameObject.name += " " + i.ToString();
                    knobs.Add(knob);

                    int index = i;
                    if (knob.TryGetComponent<ISelect>(out ISelect selectable))
                        selectable.OnSelect += (eventArgs) => ClickKnob(index);
                }
                _knobs = knobs.ToArray();
            }
        }

        #region Subscribers
        private void ClickKnob(int index)
        {
            _targetAnchorPoint = _anchorPoints[_currentKey][index];
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
        public void InitElementGroup(string key, BaseUIElement prefab, int count)
        {
            if (_scrollElements.TryGetValue(key, out List<BaseUIElement> elements))
                InstantiateGroup(elements);
            else
            {
                List<BaseUIElement> newGroup = new();
                InstantiateGroup(newGroup);
                _scrollElements.Add(key, newGroup);
            }

            if (_anchorPoints.TryGetValue(key, out float[] points))
                _anchorPoints[key] = new float[points.Length + count];
            else
                _anchorPoints = new Dictionary<string, float[]> { { key, new float[count] } };

            int childCount = _scrollElements[key].Count;
            if (_subdivisionDists.TryGetValue(key, out float dst))
                _subdivisionDists[key] = 1f / (childCount - 1);
            else
                _subdivisionDists = new Dictionary<string, float>() { { key, 1f / (childCount - 1) } };
           
            for (int i = 0; i < childCount; i++)
                _anchorPoints[_currentKey][i] = _subdivisionDists[_currentKey] * i;

            void InstantiateGroup(List<BaseUIElement> group)
            {
                for (int i = 0; i < count; i++)
                {
                    BaseUIElement element = Instantiate(prefab, _contentHolder);
                    element.gameObject.SetActive(false);
                    group.Add(element);
                }
            }
        }

        public void EnableElementGroup(string key)
        {
            if (_scrollElements.TryGetValue(_currentKey, out List<BaseUIElement> currentElements))
            {
                foreach (BaseUIElement element in currentElements)
                    element.gameObject.SetActive(false);
            }

            if (_scrollElements.TryGetValue(key, out List<BaseUIElement> elements))
            {
                foreach (BaseUIElement element in elements)
                    element.gameObject.SetActive(true);
                _currentKey = key;
            }
        }

        public void SelectPageWithIndex(int index) => ClickKnob(index);

        public int GetPageCount(string key)
        {
            if (_scrollElements.TryGetValue(key, out List<BaseUIElement> elements))
                return elements.Count;
            return 0;
        }

        #endregion

        private void Update()
        {
            if (_currentKey == null || _currentKey == string.Empty) return;

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
            for (int i = 0; i < _anchorPoints[_currentKey].Length; i++)
            {
                if (_anchorPoints[_currentKey][i] == _anchorPoint)
                {
                    OnFocusElement?.Invoke(_scrollElements[_currentKey][i], i);

                    if (!_useKnob || _knobs == null) continue;
                    OnFocusElement?.Invoke(_knobs[i], i);
                }
                else
                    OnUnfocusElement?.Invoke(_scrollElements[_currentKey][i], i);
            }
        }

        private void SetNextAnchorPoint()
        {
            _scrollbarValue = _scrollbar.value;
            if (_scrollbarValue < 0)
                _anchorPoint = 0;
            else
            {
                int childCount = _scrollElements[_currentKey].Count;
                for (int i = 0; i < childCount; i++)
                {
                    if (_scrollbarValue < _anchorPoints[_currentKey][i] + (_subdivisionDists[_currentKey] / 2) &&
                        _scrollbarValue > _anchorPoints[_currentKey][i] - (_subdivisionDists[_currentKey] / 2))
                    {
                        _anchorPoint = _anchorPoints[_currentKey][i];
                        break;
                    }
                    if (i == childCount - 1)
                        _anchorPoint = _anchorPoints[_currentKey][i];
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
