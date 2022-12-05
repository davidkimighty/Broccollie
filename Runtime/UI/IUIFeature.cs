using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public interface IUIFeature
    {
        void Execute(string state, PointerEventData eventData = null, Action done = null);

    }
}
