using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieMollie.Game
{
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public class PhysicsPlayerController : MonoBehaviour
    {
        #region Variable Field
        [Header("Player")]
        [SerializeField] private Rigidbody _targetBody = null;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private CameraController _cameraController = null;

        [Header("Float")]
        [SerializeField] private Vector3 _rayDir = Vector3.down;
        [SerializeField] private float _floatHeight = 0.6f;
        [SerializeField] private float _rayLength = 1f;
        [SerializeField] private float _rayStartPointOffset = 0.5f;
        [SerializeField] private float _springStrength = 30f;
        [SerializeField] private float _springDamper = 10f;
        [SerializeField] private LayerMask _platformLayer;

        [Header("Move")]
        [SerializeField] private float _maxSpeed = 6f;
        [SerializeField] private float _acceleration = 150f;
        [SerializeField] private AnimationCurve _accelerationFactorFromDot = null;
        [SerializeField] private float _maxAcceleration = 200f;
        [SerializeField] private AnimationCurve _maxAccelerationFactorFromDot = null;

        [Header("Rotate")]
        [SerializeField] private float _rotationStrength = 40f;
        [SerializeField] private float _rotationDamper = 6f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _fallMultiplier = 3f;

        private PlayerInputActions _inputActions = null;
        private Vector3 _moveInput = Vector3.zero;
        private bool _jumpInput = false;
        
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
            
        }
        
        private void OnEnable()
        {
            _inputActions.Player.Move.performed += ReadMoveInput;
            _inputActions.Player.Move.canceled += ReadMoveInput;
            _inputActions.Player.Jump.performed += ReadJumpInput;
            _inputActions.Player.Jump.canceled += ReadJumpInput;
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Player.Move.performed -= ReadMoveInput;
            _inputActions.Player.Move.canceled -= ReadMoveInput;
            _inputActions.Player.Jump.performed -= ReadJumpInput;
            _inputActions.Player.Jump.canceled -= ReadJumpInput;
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

        #endregion

        private void FixedUpdate()
        {
            (bool isHit, RaycastHit rayHit) = CastRay();
            Floating(isHit, rayHit);
            Rotation();
            Movement(isHit, rayHit);
            Jump(isHit, rayHit);
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
            if (_moveDirection.magnitude > 0 && _cameraController.ViewType == CameraViewType.ThirdPersonView)
            {
                _lookRotation = Quaternion.LookRotation(_moveDirection);
            }
            else if (_cameraController.ViewType == CameraViewType.FirstPersonView)
            {
                _yawAngle += _cameraController.LookVelocity.x;
                _lookRotation = Quaternion.Euler(_yawAngle * Vector3.up);
                _targetBody.transform.Rotate(_cameraController.LookVelocity.x * Vector3.up);
            }

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

    
}
