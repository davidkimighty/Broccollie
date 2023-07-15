using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Broccollie.Game
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class CharacterCamera : BaseCamera
    {
        [SerializeField] private InputActionProperty _lookInputAction;
        [SerializeField] private ViewType _cameraView = ViewType.ThirdPerson;
        public ViewType CameraView
        {
            get => _cameraView;
        }

        [Header("First-Person")]
        [SerializeField] private CinemachineVirtualCamera _fpVirtualCam = null;
        [SerializeField] private float _fpSpeedX = 120f;
        [SerializeField] private float _fpSpeedY = 60f;
        [SerializeField] private LayerMask _fpIgnoreCullingLayer;

        [Header("Third-Person")]
        [SerializeField] private CinemachineFreeLook _tpVirtualCam = null;
        [SerializeField] private float _tpSpeedX = 350f;
        [SerializeField] private float _tpSpeedY = 5f;

        private Vector2 _lookInput = Vector2.zero;
        private Vector2 _lookVelocity = Vector2.zero;
        public Vector2 LookVelocity
        {
            get => _lookVelocity;
        }

        private float _fpPitchAngle = 0f;
        public float FpPitchAngle
        {
            get => _fpPitchAngle;
        }

        private float _fpYawAngle = 0f;
        public float FpYawAngle
        {
            get => _fpYawAngle;
        }

        public override void Awake()
        {
            base.Awake();

            Cursor.lockState = CursorLockMode.Locked;

            _tpVirtualCam.m_XAxis.m_MaxSpeed = _tpSpeedX;
            _tpVirtualCam.m_YAxis.m_MaxSpeed = _tpSpeedY;

            ChangeCameraView(_cameraView);
        }

        private void OnEnable()
        {
            _lookInputAction.action.performed += ReadLookInput;
            _lookInputAction.action.canceled += ReadLookInput;
        }

        private void OnDisable()
        {
            _lookInputAction.action.performed -= ReadLookInput;
            _lookInputAction.action.canceled -= ReadLookInput;
        }

        #region Subscribers
        private void ReadLookInput(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        #endregion

        #region Public Functions
        public void ChangeCameraView(ViewType type)
        {
            _cameraView = type;
            switch (type)
            {
                case ViewType.FirstPerson:
                    Camera.cullingMask = 1 << _fpIgnoreCullingLayer;

                    _tpVirtualCam.enabled = false;
                    _fpVirtualCam.enabled = true;
                    break;

                case ViewType.ThirdPerson:
                    Camera.cullingMask = -1;

                    _fpVirtualCam.enabled = false;
                    _tpVirtualCam.enabled = true;
                    break;
            }
        }

        #endregion

        private void LateUpdate()
        {
            FirstPersonCameraMovement();
        }

        private void FirstPersonCameraMovement()
        {
            if (_cameraView != ViewType.FirstPerson) return;

            float yawVelocity = _lookInput.x * _fpSpeedX * Time.deltaTime;
            float pitchVelocity = _lookInput.y * _fpSpeedY * Time.deltaTime;
            _lookVelocity = new Vector2(yawVelocity, pitchVelocity);

            _fpYawAngle += yawVelocity;
            _fpPitchAngle -= pitchVelocity;
            _fpPitchAngle = Mathf.Clamp(_fpPitchAngle, -90f, 90f);

            _fpVirtualCam.Follow.localRotation = Quaternion.Euler(_fpPitchAngle, 0, 0);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            ChangeCameraView(_cameraView);
        }
#endif

        public enum ViewType { FirstPerson, ThirdPerson }
    }
}
