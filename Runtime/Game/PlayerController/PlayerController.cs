using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace CollieMollie.Game
{
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        #region Variable Field
        [Header("Character Movement")]
        [SerializeField] private CharacterController _characterController = null;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _accelerate = 3f;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _fallMultiplier = 3f;
        [SerializeField] private float _rotateSmoothDamp = 0.3f;

        [Header("Camera")]
        [SerializeField] private CameraViewType _cameraViewType = CameraViewType.ThirdPersonView;
        [SerializeField] private Transform _cameraLookTarget = null;

        [SerializeField] private CinemachineVirtualCamera _firstPersonVirtualCam = null;
        [SerializeField] private float _fpcRotateSpeedX = 120f;
        [SerializeField] private float _fpcRotateSpeedY = 30f;

        [SerializeField] private CinemachineFreeLook _thirdPersonVirtualCam = null;
        [SerializeField] private float _tpcRotateSpeedX = 360f;
        [SerializeField] private float _tpcRotateSpeedY = 30f;

        private PlayerInputActions _inputActions = null;
        private Vector3 _moveInput = Vector3.zero;
        private bool _jumpInput = false;
        private Vector2 _lookInput = Vector2.zero;

        private float _verticalVelocity = 0f;
        private float _horizontalVelocity = 0f;
        private float _pitchAngle = 0f;

        private Vector3 _inputDirection = Vector3.zero;
        private float _rotateVelocity = 0f;

        #endregion

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            Cursor.lockState = CursorLockMode.Locked;

            _thirdPersonVirtualCam.m_XAxis.m_MaxSpeed = _tpcRotateSpeedX;
            _thirdPersonVirtualCam.m_YAxis.m_MaxSpeed = _tpcRotateSpeedY;
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
            if (_cameraViewType == CameraViewType.FirstPersonView)
                _moveInput = _characterController.transform.right * rawInput.x + _characterController.transform.forward * rawInput.y;
            else if (_cameraViewType == CameraViewType.ThirdPersonView)
                _moveInput = rawInput;
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

        #endregion

        private void Update()
        {
            ApplyGravity();
            Move();
            Jump();
        }

        private void LateUpdate()
        {
            FirstPersonCameraRotation();
        }

        #region Private Functions
        private void ApplyGravity()
        {
            if (_characterController.isGrounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;
            }
            else
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void Jump()
        {
            if (_jumpInput && _characterController.isGrounded)
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            if (_characterController.velocity.y < 0)
                _verticalVelocity += _gravity * (_fallMultiplier - 1) * Time.deltaTime;
        }

        private void Move()
        {
            if (_cameraViewType == CameraViewType.FirstPersonView)
                FirstPersonControlMove();
            else if (_cameraViewType == CameraViewType.ThirdPersonView)
                ThirdPersonControlMove();

            void FirstPersonControlMove()
            {
                float targetSpeed = _walkSpeed;
                float currentSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;

                _horizontalVelocity = Mathf.Lerp(currentSpeed, _moveInput.magnitude * targetSpeed, _accelerate * Time.deltaTime);
                Vector3 horizontalMovement = _moveInput.normalized * _horizontalVelocity * Time.deltaTime;
                Vector3 verticalMovement = new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime;

                _characterController.Move(horizontalMovement + verticalMovement);
            }

            void ThirdPersonControlMove()
            {
                _inputDirection = Vector3.right * _moveInput.x + Vector3.forward * _moveInput.y;

                float targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                float rotateAngle = Mathf.SmoothDampAngle(_characterController.transform.eulerAngles.y, targetAngle, ref _rotateVelocity, _rotateSmoothDamp);

                if (_moveInput.magnitude > 0)
                    _characterController.transform.rotation = Quaternion.Euler(0f, rotateAngle, 0f);

                float targetSpeed = _walkSpeed;
                float currentSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;
                Vector3 moveDirection = _inputDirection.magnitude > 0 ? Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward : _characterController.transform.forward;

                _horizontalVelocity = Mathf.Lerp(currentSpeed, _inputDirection.magnitude * targetSpeed, _accelerate * Time.deltaTime);
                Vector3 horizontalMovement = moveDirection * _horizontalVelocity * Time.deltaTime;
                Vector3 verticalMovement = new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime;

                _characterController.Move(horizontalMovement + verticalMovement);
            }
        }

        private void FirstPersonCameraRotation()
        {
            if (_cameraViewType != CameraViewType.FirstPersonView) return;

            float yawVelocity = _lookInput.x * _fpcRotateSpeedX * Time.deltaTime;
            float pitchVelocity = _lookInput.y * _fpcRotateSpeedY * Time.deltaTime;

            _pitchAngle -= pitchVelocity;
            _pitchAngle = Mathf.Clamp(_pitchAngle, -90f, 90f);

            _cameraLookTarget.localRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);
            _characterController.transform.Rotate(Vector3.up * yawVelocity);
        }

        #endregion
    }
}
