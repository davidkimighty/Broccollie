using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Broccollie.System
{
    public interface ISaveable
    {
        void SaveState(object state);

        void LoadState(object state);
    }
}
