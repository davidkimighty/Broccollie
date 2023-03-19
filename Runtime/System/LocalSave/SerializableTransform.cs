using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Broccollie.System
{
    [Serializable]
    public class SerializableTransform
    {
        [SerializeField] private float[,] _sTransform = new float[2, 3];

        public SerializableTransform(Vector3 vector, Vector3 euler)
        {
            _sTransform[0, 0] = vector.x;
            _sTransform[0, 1] = vector.y;
            _sTransform[0, 2] = vector.z;

            _sTransform[1, 0] = euler.x;
            _sTransform[1, 1] = euler.y;
            _sTransform[1, 2] = euler.z;
        }

        public float[,] ToVector()
        {
            return _sTransform;
        }
    }
}
