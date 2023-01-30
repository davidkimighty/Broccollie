using UnityEngine;

namespace CollieMollie.Game
{
    public abstract class CameraEffect : ScriptableObject
    {
        public virtual void Play(MonoBehaviour mono, Transform camera) { }
    }
}