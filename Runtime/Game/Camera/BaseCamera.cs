using UnityEngine;

namespace Broccollie.Game
{
    public class BaseCamera : MonoBehaviour
    {
        [SerializeField] protected Camera _camera = null;
        public Camera Camera
        {
            get => _camera;
        }

        #region Public Functions
        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        #endregion

        public virtual void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }
    }
}
