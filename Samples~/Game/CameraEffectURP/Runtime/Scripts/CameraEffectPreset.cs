using UnityEngine;

namespace Broccollie.Game.CameraEffect
{
    public abstract class CameraEffectPreset : ScriptableObject
    {
        public virtual void Play(MonoBehaviour mono, Transform camera) { }
    }
}