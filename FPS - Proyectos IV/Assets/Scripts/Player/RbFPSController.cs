using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RbFPSController : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8f;
        public float BackwardSpeed = 4f;
        public float StrafeSpeed = 4f;
        public float RunMultiplier = 2f;

        public float JumpForce = 30f;
        [HideInInspector] public float CurrentTargetSpeed = 8f;

        private bool _Running;

        public bool Running { get => _Running; }

        public void Init()
        {
            InputManager.Instance.OnHoldRun += UpdateRunning;
        }
        private void UpdateRunning(bool isRun)
        {
            _Running = isRun;
        }

        public void UpdateDesiredTargetSpeed(Vector3 input)
        {
            if (input == Vector3.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.z < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.z > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
            if (_Running)
            {
                CurrentTargetSpeed *= RunMultiplier;
            }
        }
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public AdvancedSettings advancedSettings = new AdvancedSettings();

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private float _yRotation;
    private Vector3 _groundContactNormal;
    private bool _jump, _previouslyGrounded, _jumping, _isGrounded;

    private Vector3 _input = Vector3.zero;
    private Vector3 _mouse = Vector3.zero;

    public Vector3 Velocity { get => _rigidbody.velocity; }
    public bool Jumping { get => _jumping; }
    public bool IsGrounded { get => _isGrounded; }
    public bool Running { get => movementSettings.Running; }
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        movementSettings.Init();

        InputManager.Instance.OnMoveForward += UpdateInputZ;
        InputManager.Instance.OnMoveRight += UpdateInputX;
        InputManager.Instance.OnTriggerJump += UpdateJumpInput;
    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
        //Debug.Log(_jump);
        //Debug.Log(_isGrounded);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        //JumpingChecker();
    }

    #region Camera and View
    private void UpdateMouseX(float mouseX)
    {
        _mouse.x = mouseX;
    }
    private void UpdateMouseY(float mouseY)
    {
        _mouse.y = mouseY;
    }
    private void RotateView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        float oldYRot = transform.eulerAngles.y;

        if(_isGrounded || advancedSettings.airControl)
        {
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRot, Vector3.up);
            _rigidbody.velocity = velRotation * _rigidbody.velocity;
        }
    }
    #endregion
    #region XZ Movement
    private void UpdateInputX(float inp)
    {
        _input.x = inp;
        movementSettings.UpdateDesiredTargetSpeed(_input);
    }
    private void UpdateInputZ(float inp)
    {
        _input.z = inp;
        movementSettings.UpdateDesiredTargetSpeed(_input);
    }
    private void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input * movementSettings.CurrentTargetSpeed * Time.fixedDeltaTime);
    }
    #endregion
    #region Jump
    private void UpdateJumpInput()
    {
        //_jump = true;
        if (_isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(movementSettings.JumpForce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }
    private void JumpingChecker()
    {
        if (_isGrounded)
        {
            _rigidbody.drag = 5f;

            if (_jump)
            {
                Debug.Log("jump");

                _rigidbody.drag = 0f;
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
                _rigidbody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                _jumping = true;
            }

            if (_jumping && Mathf.Abs(_input.x) < float.Epsilon && Mathf.Abs(_input.y) < float.Epsilon && _rigidbody.velocity.magnitude < 1f)
            {
                _rigidbody.Sleep();
            }
        }
        else
        {
            _rigidbody.drag = 0f;
            if (_previouslyGrounded && _jumping)
            {
                StickToGroundHelper();
            }
        }
        _jump = false;
    }

    private void StickToGroundHelper()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _collider.radius*(1f-advancedSettings.shellOffset), Vector3.down, out hit,
            ((_collider.height/2f)-_collider.radius)+
            advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hit.normal, Vector3.up)) < 85f)
            {
                _rigidbody.velocity = Vector3.ProjectOnPlane(_rigidbody.velocity, hit.normal);
            }
        }
    }

    private void GroundCheck()
    {
        _previouslyGrounded = _isGrounded;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position,_collider.radius*(1f - advancedSettings.shellOffset), Vector3.down, out hit,
            ((_collider.height / 2f) - _collider.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            _isGrounded = true;
            _groundContactNormal = hit.normal;
            Debug.Log("grounded");
        }
        else
        {
            Debug.Log("not grounded");
            _isGrounded = false;
            _groundContactNormal = Vector3.up;
        }
        if (!_previouslyGrounded && _isGrounded && _jumping)
        {
            _jumping = false;
        }
    }
    #endregion
}
