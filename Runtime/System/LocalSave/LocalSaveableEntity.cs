using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Broccollie.System
{
    public class LocalSaveableEntity : MonoBehaviour
    {
        #region Variable Field
        private static Dictionary<string, LocalSaveableEntity> s_globalSaveables = new Dictionary<string, LocalSaveableEntity>();

        [SerializeField] private ScriptableObject _saveObject = null;
        [SerializeField] private string _uniqueId = null;
        public string UniqueId
        {
            get => _uniqueId;
        }

        private ISaveable[] _saveables = null;

        #endregion

        private void Awake()
        {
            _saveables = GetComponents<ISaveable>();
        }

        #region Public Functions
        public void SaveStates()
        {
            if (_saveables == null) return;

            foreach (ISaveable saveable in _saveables)
            {
                saveable.SaveState(_saveObject);
            }
        }

        public void LoadStates()
        {
            if (_saveables == null || _saveObject == null) return;

            foreach (ISaveable saveable in _saveables)
            {
                saveable.LoadState(_saveObject);
            }
        }

#if UNITY_EDITOR
        public void Initialize()
        {
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; // Has been placed?

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_uniqueId");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = GetNewGuid();
                serializedObject.ApplyModifiedProperties();
            }
            s_globalSaveables[property.stringValue] = this;
        }
#endif

        [ContextMenu("Set New Guid")]
        public string GetNewGuid()
        {
            _uniqueId = Guid.NewGuid().ToString();
            return _uniqueId;
        }
        #endregion

        #region Private Functions
        private bool IsUnique(string candidate)
        {
            if (!s_globalSaveables.ContainsKey(candidate)) return true;

            if (s_globalSaveables[candidate] == this) return true;

            if (s_globalSaveables[candidate] == null)
            {
                s_globalSaveables.Remove(candidate);
                return true;
            }

            if (s_globalSaveables[candidate].UniqueId != candidate)
            {
                s_globalSaveables.Remove(candidate);
                return true;
            }
            return false;
        }

        #endregion
    }
}