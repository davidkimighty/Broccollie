using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CollieMollie.Core
{
    public static partial class Helper
    {
        #region Vector


        #endregion

        #region Quaternion
        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            if (Quaternion.Dot(a, b) < 0)
                return a * Quaternion.Inverse(Multiply(b, -1f));
            return a * Quaternion.Inverse(b);
        }

        public static Quaternion Multiply(Quaternion q, float scalar)
        {
            return new Quaternion(q.x * scalar, q.y * scalar, q.z * scalar, q.w * scalar);
        }

        #endregion
    }
}
