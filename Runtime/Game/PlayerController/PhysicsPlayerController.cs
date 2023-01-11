using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieMollie.Game
{
    [DisallowMultipleComponent]
    public class PhysicsPlayerController : MonoBehaviour
    {
        #region Variable Field
        [Header("Physics")]
        [SerializeField] private Rigidbody _targetBody = null;
        [SerializeField] private LayerMask _playerLayer;

        [Header("Camera")]
        [SerializeField] private CameraViewType _cameraViewType = CameraViewType.ThirdPersonView;
        [SerializeField] private Transform _cameraLookTarget = null;
        [SerializeField] private bool _lockCameraMovement = false;

        [Header("First Person View")]
        [SerializeField] private CinemachineVirtualCamera _firstPersonVirtualCam = null;
        [SerializeField] private float _firstPersonCameraSpeedX = 120f;
        [SerializeField] private float _firstPersonCameraSpeedY = 60f;

        [Header("Third Person View")]
        [SerializeField] private CinemachineFreeLook _thirdPersonVirtualCam = null;
        [SerializeField] private float _thirdPersonCameraSpeedX = 360f;
        [SerializeField] private float _thirdPersonCameraSpeedY = 6f;

        [Header("Float")]
        [SerializeField] private Vector3 _rayDir = Vector3.down;
        [SerializeField] private float _floatHeight = 0.6f;
        [SerializeField] private float _rayLength = 1f;
        [SerializeField] private float _rayStartPointOffset = 0.5f;
        [SerializeField] private float _springStrength = 30f;
        [SerializeField] private float _springDamper = 10f;
        [SerializeField] private LayerMask _platformLayer;

        [Header("Rotation")]
        [SerializeField] private float _rotationStrength = 40f;
        [SerializeField] private float _rotationDamper = 6f;

        [Header("Move")]
        [SerializeField] private float _maxSpeed = 6f;
        [SerializeField] private float _acceleration = 150f;
        [SerializeField] private AnimationCurve _accelerationFactorFromDot = null;
        [SerializeField] private float _maxAcceleration = 200f;
        [SerializeField] private AnimationCurve _maxAccelerationFactorFromDot = null;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _fallMultiplier = 3f;

        private PlayerInputActions _inputActions = null;
        private Vector3 _moveInput = Vector3.zero;
        private bool _jumpInput = false;
        private Vector2 _lookInput = Vector2.zero;

        private Vector3 _moveDirection = Vector3.zero;
        private Quaternion _lookRotation = Quaternion.identity;
        private Vector3 _targetVel = Vector3.zero;
        private bool _grounded = true;
        private float _pitchAngle = 0f;
        private float _yawAngle = 0f;

        #endregion

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            Cursor.lockState = CursorLockMode.Locked;

            _thirdPersonVirtualCam.Follow = _cameraLookTarget;
            _thirdPersonVirtualCam.LookAt = _cameraLookTarget;
            _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = _thirdPersonCameraSpeedX;
            _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = _thirdPersonCameraSpeedY;

            _firstPersonVirtualCam.Follow = _cameraLookTarget;
        }

        private void Start()
        {
            LockCameraMovement(_lockCameraMovement);
            ChangeCameraView(_cameraViewType);
        }

        private void OnEnable()
        {
            _inputActions.Player.Move.performed += ReadMoveInput;
            _inputActions.Player.Move.canceled += ReadMoveInput;
            _inputActions.Player.Jump.performed += ReadJumpInput;
            _inputActions.Player.Jump.canceled += ReadJumpInput;
            _inputActions.Player.Look.performed += ReadLookInput;
            _inputActions.Player.Look.canceled += ReadLookInput;
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Move.performed -= ReadMoveInput;
            _inputActions.Player.Move.canceled -= ReadMoveInput;
            _inputActions.Player.Jump.performed -= ReadJumpInput;
            _inputActions.Player.Jump.canceled -= ReadJumpInput;
            _inputActions.Player.Look.performed -= ReadLookInput;
            _inputActions.Player.Look.canceled -= ReadLookInput;
            _inputActions.Disable();
        }

        #region Subscribers
        public void ReadMoveInput(InputAction.CallbackContext context)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            _moveInput = Vector3.right * rawInput.x + Vector3.forward * rawInput.y;
        }

        private void ReadJumpInput(InputAction.CallbackContext context)
        {
            _jumpInput = context.ReadValueAsButton();
        }

        private void ReadLookInput(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        #endregion

        #region Public Functions
        public void ChangeCameraView(CameraViewType type)
        {
            _cameraViewType = type;
            switch (type)
            {
                case CameraViewType.FirstPersonView:
                    _firstPersonVirtualCam.gameObject.SetActive(true);
                    _thirdPersonVirtualCam.gameObject.SetActive(false);
                    break;

                case CameraViewType.ThirdPersonView:
                    _thirdPersonVirtualCam.gameObject.SetActive(true);
                    _firstPersonVirtualCam.gameObject.SetActive(false);
                    break;
            }
        }

        public void LockCameraMovement(bool state)
        {
            _lockCameraMovement = state;
            switch (_cameraViewType)
            {
                case CameraViewType.FirstPersonView:
                    _firstPersonVirtualCam.gameObject.SetActive(true);
                    _thirdPersonVirtualCam.gameObject.SetActive(false);
                    break;

                case CameraViewType.ThirdPersonView:
                    if (state)
                    {
                        _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = 0;
                        _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = 0;
                    }
                    else
                    {
                        _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = _thirdPersonCameraSpeedX;
                        _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = _thirdPersonCameraSpeedY;
                    }
                    break;
            }
        }

        #endregion

        private void FixedUpdate()
        {
            (bool isHit, RaycastHit rayHit) = CastRay();
            Floating(isHit, rayHit);
            Rotation();
            Movement(isHit, rayHit);
            Jump(isHit, rayHit);
        }

        private void LateUpdate()
        {
            FirstPersonCameraMovement();
        }

        #region Private Functions
        private (bool, RaycastHit) CastRay()
        {
            Vector3 startPoint = _targetBody.position;
            startPoint.y += _rayStartPointOffset;
            Ray ray = new Ray(startPoint, Vector3.down);
            bool isHit = Physics.Raycast(ray, out RaycastHit rayHit, _rayLength + _rayStartPointOffset, ~_playerLayer);
            return (isHit, rayHit);
        }

        private void Floating(bool isHit, RaycastHit rayHit)
        {
            if (isHit)
            {
                _grounded = rayHit.distance <= _floatHeight + _rayStartPointOffset;
                if (_grounded)
                {
                    Vector3 vel = _targetBody.velocity;
                    Vector3 hitVel = rayHit.rigidbody != null ? rayHit.rigidbody.velocity : Vector3.zero;

                    float rayDirVel = Vector3.Dot(_rayDir, vel);
                    float hitRayDirVel = Vector3.Dot(_rayDir, hitVel);
                    float relVel = rayDirVel - hitRayDirVel;
                    float offset = rayHit.distance - _floatHeight - _rayStartPointOffset;
                    float springForce = (offset * _springStrength) - (relVel * _springDamper);
                    _targetBody.AddForce(_rayDir * springForce);

                    if (rayHit.rigidbody != null)
                        rayHit.rigidbody.AddForceAtPosition(_rayDir * -springForce, rayHit.point);
                }
            }
            else
            {
                if (_grounded)
                    _grounded = false;
            }
        }

        private void Rotation()
        {
            _moveDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * _moveInput;
            if (_moveDirection.magnitude > 0 && _cameraViewType == CameraViewType.ThirdPersonView)
                _lookRotation = Quaternion.LookRotation(_moveDirection);

            Quaternion currentRotation = _targetBody.rotation;
            Quaternion targetRotation = Helper.Helper.ShortestRotation(_lookRotation, currentRotation);

            targetRotation.ToAngleAxis(out float angle, out Vector3 axis);
            float rotationRadians = angle * Mathf.Deg2Rad;
            _targetBody.AddTorque((axis.normalized * (rotationRadians * _rotationStrength)) - (_targetBody.angularVelocity * _rotationDamper));
        }

        private void Movement(bool isHit, RaycastHit rayHit)
        {
            float velDot = Vector3.Dot(_moveDirection, _targetVel.normalized);
            float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot);
            Vector3 targetVel = _moveDirection * _maxSpeed;
            _targetVel = Vector3.MoveTowards(_targetVel, targetVel, accel * Time.fixedDeltaTime);

            Vector3 targetAccel = (_targetVel - _targetBody.velocity) / Time.fixedDeltaTime;
            float maxAccel = _maxAcceleration * _maxAccelerationFactorFromDot.Evaluate(velDot);
            targetAccel = Vector3.ClampMagnitude(targetAccel, maxAccel);

            Vector3 force = Vector3.Scale(targetAccel * _targetBody.mass, new Vector3(1, 0, 1));
            //_targetBody.AddForceAtPosition(force, _targetBody.position);
            _targetBody.AddForce(force);
        }

        private void Jump(bool isHit, RaycastHit rayHit)
        {
            if (!_grounded)
            {
                if (_targetBody.velocity.y < 0)
                    _targetBody.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
                return;
            }

            if (_jumpInput)
            {
                _targetBody.velocity = new Vector3(_targetBody.velocity.x, 0, _targetBody.velocity.z);
                _targetBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }

        //private void PlatformInteraction(bool isHit, RaycastHit rayHit)
        //{
        //    void GetOnPlatform()
        //    {
        //        int layerMask = 1 << rayHit.transform.gameObject.layer;
        //        if ((layerMask & _platformLayer.value) != 0)
        //            _targetBody.transform.SetParent(rayHit.transform);
        //    }

        //    void GetOffPlatform()
        //    {
        //        if (_targetBody.transform.parent != null)
        //            _targetBody.transform.SetParent(null);
        //    }
        //}

        private void FirstPersonCameraMovement()
        {
            if (_cameraViewType != CameraViewType.FirstPersonView) return;

            float yawVelocity = _lookInput.x * _firstPersonCameraSpeedX * Time.deltaTime;
            float pitchVelocity = _lookInput.y * _firstPersonCameraSpeedY * Time.deltaTime;

            _pitchAngle -= pitchVelocity;
            _pitchAngle = Mathf.Clamp(_pitchAngle, -90f, 90f);
            _cameraLookTarget.localRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);

            _yawAngle += yawVelocity;
            _lookRotation = Quaternion.Euler(_yawAngle * Vector3.up);
            _targetBody.transform.Rotate(yawVelocity * Vector3.up);
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawMoveDirectionRay();
            DrawFloatingRay();

            void DrawMoveDirectionRay()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(_targetBody.position, _moveDirection);
            }

            void DrawFloatingRay()
            {
                Gizmos.color = Color.white;
                Gizmos.DrawRay(_targetBody.position, (_rayLength + _rayStartPointOffset) * _rayDir);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(_targetBody.position, (_floatHeight + _rayStartPointOffset) * _rayDir);
            }

        }
#endif
    }

    public enum CameraViewType { ThirdPersonView, FirstPersonView }
}
