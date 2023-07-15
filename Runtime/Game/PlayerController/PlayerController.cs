using UnityEngine;
using UnityEngine.InputSystem;

namespace Broccollie.Game
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private CharacterController _characterController = null;
        [SerializeField] private CharacterCamera _characterCam = null;

        [Header("Move")]
        [SerializeField] private InputActionProperty _moveAction;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _walkSpeed = 3f;
        [SerializeField] private float _accelerate = 3f;

        [Header("Rotate")]
        [SerializeField] private float _rotateSmoothDamp = 0.3f;

        [Header("Jump")]
        [SerializeField] private InputActionProperty _jumpAction;
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _fallMultiplier = 3f;

        private Vector3 _moveInput = Vector3.zero;
        private bool _jumpInput = false;

        private float _verticalVelocity = 0f;
        private float _horizontalVelocity = 0f;

        private Vector3 _inputDirection = Vector3.zero;
        private float _rotateVelocity = 0f;

        private void OnEnable()
        {
            _moveAction.action.performed += ReadMoveInput;
            _moveAction.action.canceled += ReadMoveInput;
            _jumpAction.action.performed += ReadJumpInput;
            _jumpAction.action.canceled += ReadJumpInput;
        }

        private void OnDisable()
        {
            _moveAction.action.performed -= ReadMoveInput;
            _moveAction.action.canceled -= ReadMoveInput;
            _jumpAction.action.performed -= ReadJumpInput;
            _jumpAction.action.canceled -= ReadJumpInput;
        }

        #region Subscribers
        public void ReadMoveInput(InputAction.CallbackContext context)
        {
            Vector2 rawInput = context.ReadValue<Vector2>();
            if (_characterCam.CameraView == CharacterCamera.ViewType.FirstPerson)
                _moveInput = _characterController.transform.right * rawInput.x + _characterController.transform.forward * rawInput.y;
            else if (_characterCam.CameraView == CharacterCamera.ViewType.ThirdPerson)
                _moveInput = rawInput;
        }

        private void ReadJumpInput(InputAction.CallbackContext context)
        {
            _jumpInput = context.ReadValueAsButton();
        }

        #endregion

        private void Update()
        {
            ApplyGravity();
            Move();
            Rotate();
            Jump();
        }

        private void ApplyGravity()
        {
            if (_characterController.isGrounded)
            {
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;
            }
            else
                _verticalVelocity += _gravity * Time.deltaTime;
        }

        private void Move()
        {
            if (_characterCam.CameraView == CharacterCamera.ViewType.FirstPerson)
                FirstPersonControlMove();
            else if (_characterCam.CameraView == CharacterCamera.ViewType.ThirdPerson)
                ThirdPersonControlMove();

            void FirstPersonControlMove()
            {
                float targetSpeed = _walkSpeed;
                float currentSpeed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;
                _horizontalVelocity = Mathf.Lerp(currentSpeed, _moveInput.magnitude * targetSpeed, _accelerate * Time.deltaTime);

                Vector3 moveDirection = Quaternion.Euler(0f, _characterCam.Camera.transform.eulerAngles.y, 0f) * Vector3.forward;
                Vector3 horizontalMovement = moveDirection * _horizontalVelocity * Time.deltaTime;
                Vector3 verticalMovement = new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime;

                _characterController.Move(horizontalMovement + verticalMovement);
            }

            void ThirdPersonControlMove()
            {
                _inputDirection = Vector3.right * _moveInput.x + Vector3.forward * _moveInput.y;

                float targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _characterCam.Camera.transform.eulerAngles.y;
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

        private void Rotate()
        {
            if (_characterCam.CameraView == CharacterCamera.ViewType.FirstPerson)
                _characterController.transform.Rotate(_characterCam.LookVelocity.x * Vector3.up);
        }

        private void Jump()
        {
            if (_jumpInput && _characterController.isGrounded)
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            if (_characterController.velocity.y < 0)
                _verticalVelocity += _gravity * (_fallMultiplier - 1) * Time.deltaTime;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_characterCam == null) return;
            DrawMoveDirectionRay();

            void DrawMoveDirectionRay()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(_characterCam.transform.position, _moveInput);
            }

        }
#endif
    }
}
