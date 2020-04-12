using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RbFPSController : MonoBehaviour
{
    #region Settings classes
    [System.Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float minimumX = -90F;
        public float maximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion _characterTargetRot;
        private Quaternion _cameraTargetRot;
        private bool _cursorIsLocked = true;

        public void Init(Transform character, Transform camera)
        {
            _characterTargetRot = character.localRotation;
            _cameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            _characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            _cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                _cameraTargetRot = ClampRotationAroundXAxis(_cameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, _characterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, _cameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = _characterTargetRot;
                camera.localRotation = _cameraTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _cursorIsLocked = true;
            }

            if (_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, minimumX, maximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }

    [System.Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8f;
        public float BackwardSpeed = 4f;
        public float StrafeSpeed = 4f;
        public float RunMultiplier = 2f;

        public float JumpForce = 30f;
        public float DashDistance = 5f;
        /*[HideInInspector]*/ public float CurrentTargetSpeed = 8f;

        private Vector3 lastInput = Vector3.zero;

        private bool _running;

        public bool Running { get => _running; }

        public void Init()
        {
            InputManager.Instance.OnHoldRun += IsRunning;
        }
        private void IsRunning(bool hold)
        {
            _running = hold;
        }

        public void UpdateDesiredTargetSpeed(Vector3 input)
        {
            if (input == Vector3.zero) return;
            input = new Vector3(Mathf.Abs(input.x), 0, Mathf.Abs(input.z));

            Vector2 finalInput = new Vector2(input.x * (input.x / (input.x + input.z)), input.z * (input.z / (input.x + input.z)));
            
            if(input.z > 0)
            {
                CurrentTargetSpeed = StrafeSpeed * finalInput.x + ForwardSpeed * finalInput.y;
            }
            else if (input.z < 0)
            {
                CurrentTargetSpeed = StrafeSpeed * finalInput.x + BackwardSpeed * finalInput.y;
            }
            else
            {
                CurrentTargetSpeed = StrafeSpeed;
            }

            if (!WeaponManager.Instance.Weapons[WeaponManager.Instance.selectedWeapon].GetComponent<WeaponBase>().IsAiming && _running)
            {
                CurrentTargetSpeed *= RunMultiplier;
            }
            else if (WeaponManager.Instance.Weapons[WeaponManager.Instance.selectedWeapon].GetComponent<WeaponBase>().IsAiming)
            {
                CurrentTargetSpeed /= WeaponManager.Instance.Weapons[WeaponManager.Instance.selectedWeapon].GetComponent<WeaponBase>().SpeedDecreaseWhenAim;
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
    #endregion

    #region Variables and properties
    public Camera cam;
    public MovementSettings movementSettings = new MovementSettings();
    public AdvancedSettings advancedSettings = new AdvancedSettings();
    public MouseLook mouseLook = new MouseLook();
    [SerializeField] protected float smoothXZDrag = 10f;

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
    #endregion

    #region Unity API Methods
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        movementSettings.Init();
        mouseLook.Init(transform, cam.transform);

        InputManager.Instance.OnMoveForward += UpdateInputZ;
        InputManager.Instance.OnMoveRight += UpdateInputX;
        InputManager.Instance.OnTriggerJump += UpdateJumpInput;
        InputManager.Instance.OnTriggerDash += Dash;
        InputManager.Instance.OnMouseX += UpdateMouseX;
        InputManager.Instance.OnMouseY += UpdateMouseY;

    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        XZDrag();
    }
    #endregion

    private void XZDrag()
    {
        Vector3 velocity = _rigidbody.velocity;
        Vector3 xzVelocity = velocity;
        xzVelocity = Vector3.Lerp(xzVelocity, Vector3.zero, smoothXZDrag * Time.deltaTime);
        xzVelocity.y = velocity.y;
        _rigidbody.velocity = xzVelocity;
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
        mouseLook.LookRotation(transform, cam.transform);

        if (_isGrounded || advancedSettings.airControl)
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
        Vector3 moveDirection = _input * movementSettings.CurrentTargetSpeed * Time.fixedDeltaTime;
        moveDirection = transform.TransformDirection(moveDirection);
        _rigidbody.MovePosition(_rigidbody.position + moveDirection);
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
    /*private void JumpingChecker()
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
    }*/

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
            //Debug.Log("grounded");
        }
        else
        {
            //Debug.Log("not grounded");
            _isGrounded = false;
            _groundContactNormal = Vector3.up;
        }
        if (!_previouslyGrounded && _isGrounded && _jumping)
        {
            _jumping = false;
        }
    }
    #endregion
    #region Dash
    private void Dash()
    {
        Vector3 dashVelocity = Vector3.Scale(transform.forward, 
            movementSettings.DashDistance * smoothXZDrag * new Vector3(1, 0, 1));

        _rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
    }
    #endregion
}
