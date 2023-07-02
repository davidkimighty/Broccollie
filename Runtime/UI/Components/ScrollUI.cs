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
        public event Action<BaseUI, int> OnUnfocusElement = null;

        [SerializeField] private string _preSetupElementsKey = "pre";
        [SerializeField] private List<BaseUI> _preSetupScrollElements = new List<BaseUI>();
        public List<BaseUI> ScrollElements
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
        [SerializeField] private BaseUI _knobPrefab = null;
        [SerializeField] private Transform _knobHolder = null;

        private Dictionary<string, List<BaseUI>> _scrollElements = new();
        private Dictionary<string, float[]> _anchorPoints = new();
        private Dictionary<string, float> _subdivisionDists = new();
        private string _currentKey = "";
        private float _anchorPoint = 0f;

        private float _scrollbarValue = 0f;
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
            int childCount = _preSetupScrollElements.Count;
            if (childCount == 0) return;

            _anchorPoints = new Dictionary<string, float[]> { { _preSetupElementsKey, new float[childCount] } };
            _subdivisionDists = new Dictionary<string, float> { { _preSetupElementsKey, 1f / (childCount - 1) } };

            for (int i = 0; i < childCount; i++)
                _anchorPoints[_preSetupElementsKey][i] = _subdivisionDists[_preSetupElementsKey] * i;

            _scrollElements = new Dictionary<string, List<BaseUI>>() { { _preSetupElementsKey, _preSetupScrollElements } };
            _currentKey = _preSetupElementsKey;

            if (_useKnob)
            {
                if (_knobHolder.childCount > 0)
                {
                    for (int i = _knobHolder.childCount - 1; i >= 0; i--)
                        Destroy(_knobHolder.GetChild(i).gameObject);
                    _knobs = null;
                }

                List<BaseUI> knobs = new List<BaseUI>();
                for (int i = 0; i < childCount; i++)
                {
                    BaseUI knob = Instantiate(_knobPrefab, _knobHolder);
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
        public void InitElementGroup(string key, BaseUI prefab, int count)
        {
            if (_scrollElements.TryGetValue(key, out List<BaseUI> elements))
                InstantiateGroup(elements);
            else
            {
                List<BaseUI> newGroup = new List<BaseUI>();
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

            void InstantiateGroup(List<BaseUI> group)
            {
                for (int i = 0; i < count; i++)
                {
                    BaseUI element = Instantiate(prefab, _contentHolder);
                    element.gameObject.SetActive(false);
                    group.Add(element);
                }
            }
        }

        public void EnableElementGroup(string key)
        {
            if (_scrollElements.TryGetValue(_currentKey, out List<BaseUI> currentElements))
            {
                foreach (BaseUI element in currentElements)
                    element.gameObject.SetActive(false);
            }

            if (_scrollElements.TryGetValue(key, out List<BaseUI> elements))
            {
                foreach (BaseUI element in elements)
                    element.gameObject.SetActive(true);
                _currentKey = key;
            }
        }

        public void SelectPageWithIndex(int index) => ClickKnob(index);

        public int GetPageCount(string key)
        {
            if (_scrollElements.TryGetValue(key, out List<BaseUI> elements))
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
