using UnityEngine;

namespace Broccollie.Game
{
    public abstract class CameraEffectPreset : ScriptableObject
    {
        public virtual void Play(MonoBehaviour mono, Transform camera) { }
    }
}