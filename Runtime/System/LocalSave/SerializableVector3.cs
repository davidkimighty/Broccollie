using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Broccollie.System
{
    [Serializable]
    public class SerializableVector3
    {
        [SerializeField] private float _x, _y, _z;

        public SerializableVector3(Vector3 vector)
        {
            _x = vector.x;
            _y = vector.y;
            _z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(_x, _y, _z);
        }
    }

}
