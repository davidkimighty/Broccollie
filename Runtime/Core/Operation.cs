using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Core
{
    public class Operation
    {
        #region Variable Field
        private IEnumerator _operation = null;
        private List<IEnumerator> _subOperations = new List<IEnumerator>();

        #endregion

        #region Public Functions
        public void Add(IEnumerator subOperation)
        {
            _subOperations.Add(subOperation);
        }

        public void Remove(IEnumerator subOperation)
        {
            if (!_subOperations.Contains(subOperation)) return;
            _subOperations.Remove(subOperation);
        }

        public void Start(MonoBehaviour mono, float duration, Action done = null)
        {
            if (_operation != null)
                mono.StopCoroutine(_operation);

            _operation = StartSubOperations();
            mono.StartCoroutine(_operation);

            IEnumerator StartSubOperations()
            {
                foreach (IEnumerator so in _subOperations)
                    mono.StartCoroutine(so);

                yield return new WaitForSeconds(duration);
                done?.Invoke();
            }
        }

        public void Stop(MonoBehaviour mono)
        {
            if (_operation == null) return;

            foreach (IEnumerator so in _subOperations)
                mono.StopCoroutine(so);
            _subOperations.Clear();

            if (_operation != null)
                mono.StopCoroutine(_operation);
        }
        #endregion
    }
}
