using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.UI
{
    [CreateAssetMenu(fileName = "UIDragPreset", menuName = "CollieMollie/UI/DragPreset")]
    public class UIDragPreset : ScriptableObject
    {
        [Range(0, 1)]
        public float DragSpeed = 0.1f;
    }
}
