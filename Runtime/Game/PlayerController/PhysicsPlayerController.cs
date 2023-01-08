using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieMollie.Game
{
    public class PhysicsPlayerController : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private Rigidbody _targetBody = null;

        [Header("Float")]
        [SerializeField] private Vector3 _rayDir = Vector3.down;
        [SerializeField] private float _floatHeight = 0.6f;
        [SerializeField] private float _rayLength = 1f;
        [SerializeField] private float _springStrength = 30f;
        [SerializeField] private float _springDamper = 10f;
        [SerializeField] private LayerMask _platformLayer;

        [Header("Up Straight")]
        [SerializeField] private float _upStraightStrength = 40f;
        [SerializeField] private float _upStraightDamper = 5f;

        [Header("Move")]
        [SerializeField] private float _maxSpeed = 6f;
        [SerializeField] private float _acceleration = 150f;
        [SerializeField] private AnimationCurve _accelerationFactorFromDot = null;
        [SerializeField] private float _maxAcceleration = 200f;
        [SerializeField] private AnimationCurve _maxAccelerationFactorFromDot = null;

        private PlayerInputActions _inputActions = null;
        private Vector3 _moveInput = Vector3.zero;
        private float _jumpInput = 0;

        private Vector3 _targetVel = Vector3.zero;
        private bool _grounded = true;

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
            _moveInput = context.ReadValue<Vector2>();
        }

        private void ReadJumpInput(InputAction.CallbackContext context)
        {
            _jumpInput = context.ReadValue<float>();
        }

        #endregion

        private void FixedUpdate()
        {
            (bool isHit, RaycastHit rayHit) = CastRay();
            Floating(isHit, rayHit);
            UpStraight();
            Movement();
            Jump();
        }

        #region Private Functions
        private (bool, RaycastHit) CastRay()
        {
            Ray ray = new Ray(_targetBody.position, Vector3.down);
            bool isHit = Physics.Raycast(ray, out RaycastHit rayHit, _rayLength);
            return (isHit, rayHit);
        }

        private void Floating(bool isHit, RaycastHit rayHit)
        {
            if (isHit)
            {
                int layerMask = 1 << rayHit.transform.gameObject.layer;
                if ((layerMask & _platformLayer.value) != 0)
                    _targetBody.transform.SetParent(rayHit.transform);

                _grounded = rayHit.distance <= _floatHeight * 1.3f;
                if (_grounded)
                {
                    Vector3 vel = _targetBody.velocity;
                    Vector3 hitVel = rayHit.rigidbody != null ? rayHit.rigidbody.velocity : Vector3.zero;

                    float rayDirVel = Vector3.Dot(_rayDir, vel);
                    float hitRayDirVel = Vector3.Dot(_rayDir, hitVel);
                    float relVel = rayDirVel - hitRayDirVel;
                    float offset = rayHit.distance - _floatHeight;
                    float springForce = (offset * _springStrength) - (relVel * _springDamper);
                    _targetBody.AddForce(_rayDir * springForce);

                    if (rayHit.rigidbody != null)
                        rayHit.rigidbody.AddForceAtPosition(_rayDir * -springForce, rayHit.point);
                }
            }
            else
            {
                if (_targetBody.transform.parent != null)
                    _targetBody.transform.SetParent(null);
            }
        }

        private void UpStraight()
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Helper.Helper.ShortestRotation(Quaternion.identity, currentRotation);

            targetRotation.ToAngleAxis(out float angle, out Vector3 axis);
            axis.Normalize();

            float rotationRadians = angle * Mathf.Deg2Rad;
            _targetBody.AddTorque((axis * (rotationRadians * _upStraightStrength)) - (_targetBody.angularVelocity * _upStraightDamper));
        }

        private void Movement()
        {
            Vector3 moveDir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(_moveInput.x, 0, _moveInput.y);
            float velDot = Vector3.Dot(moveDir, _targetVel.normalized);

            float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot);
            Vector3 targetVel = moveDir * _maxSpeed;
            _targetVel = Vector3.MoveTowards(_targetVel, targetVel, accel * Time.fixedDeltaTime);

            Vector3 targetAccel = (_targetVel - _targetBody.velocity) / Time.fixedDeltaTime;
            float maxAccel = _maxAcceleration * _maxAccelerationFactorFromDot.Evaluate(velDot);
            targetAccel = Vector3.ClampMagnitude(targetAccel, maxAccel);

            Vector3 force = Vector3.Scale(targetAccel * _targetBody.mass, new Vector3(1, 0, 1));
            _targetBody.AddForceAtPosition(force, _targetBody.centerOfMass);
        }

        private void Jump()
        {

        }

        #endregion
    }
}
