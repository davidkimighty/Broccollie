using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class BaseUIFeature : MonoBehaviour
    {
        public virtual void Execute(string state, Action done = null) { }

        public virtual void Execute(string state, out float duration, Action done = null) { duration = 0; }

        public virtual void Execute(PointerEventData eventData = null, Action done = null) { }
    }
}
