using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CollieMollie.UI
{
    public abstract class BaseUIFeature : MonoBehaviour
    {
        public virtual async Task ExecuteAsync(string state, CancellationTokenSource token, Action done = null)
        {
            await Task.Yield();
        }

        public virtual async Task ExecuteAsync(PointerEventData eventData = null, Action done = null)
        {
            await Task.Yield();
        }

        public bool IsValid(BaseUI.State state)
        {
            return state != BaseUI.State.None;
        }
    }
}
