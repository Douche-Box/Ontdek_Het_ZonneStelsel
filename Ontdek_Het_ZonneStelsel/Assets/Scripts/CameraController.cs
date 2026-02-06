using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("References")]
    #region References

    [SerializeField] private Camera _camera;

    [SerializeField] private PlayerInput _playerInput;
    public PlayerInput PlayerInput => _playerInput;

    [SerializeField] private Transform _orientation;

    [SerializeField] private Transform _lookAtTarget;
    [SerializeField] private Vector3 _offsetToTarget;

    [SerializeField] private LayerMask _planetMask;

    #endregion

    [Header("Inputs")]
    #region Inputs

    [SerializeField] private Vector2 _cameraInput;
    [SerializeField] private float _yRotation;
    [SerializeField] private float _xRotation;

    [SerializeField] private float _zoomInput;

    [SerializeField] private Vector2 _moveInput;
    [SerializeField] private bool _isMoving;

    [SerializeField] private float _upDownInput;

    #endregion

    [Header("Settings")]

    [Header("Camera")]
    #region Camera

    [SerializeField] private float _rotationSpeed = 50.0f;
    [SerializeField] private float _moveSpeed = 5.0f;

    [SerializeField] private float _minYClamp = -89.9f;
    [SerializeField] private float _maxYClamp = 89.9f;

    [Header("Look At Target")]
    #region Look At Target

    [SerializeField] private float _lookAtDefaultOffset = 2.0f;

    [SerializeField] private float _parentDistance = 5f;
    [SerializeField] private bool _isParented = false;

    [SerializeField] private float _distanceFromTarget;
    [SerializeField] private float _focusSmoothSpeed = 5.0f;

    [SerializeField] private Transform _targetPin;

    #endregion

    #endregion

    [Header("Zoom")]
    #region Zoom

    [SerializeField] private float _zoomSpeed = 10f;

    [Header("Zoom Distance")]
    [SerializeField] private float _currentZoom;
    [Header("")]
    [SerializeField] private float _targetZoom;
    [SerializeField] private float _minZoom = -3f;
    [SerializeField] private float _maxZoom = 3f;


    [Header("Zoom FOV")]
    [SerializeField] private float _currentFOV;

    [Header("")]
    [SerializeField] private float _zoomedOutFOV = 90f;
    [SerializeField] private float _zoomedInFOV = 30f;

    #endregion

    [Header("Movement")]
    #region Movement

    [SerializeField] private float _currentFlySpeed;

    [SerializeField] private float _minFlySpeed = 1f;
    [SerializeField] private float _maxFlySpeed = 20f;

    [SerializeField] private float _flySpeedMultiplier = 1f;

    [SerializeField] private float _flySpeedChangeRate = 1f;

    private Vector3 _previousMoveDirection;

    #endregion

    #region Subscriptions

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Camera").performed += OnCamera;
        _playerInput.actions.FindAction("Camera").canceled += OnCamera;

        _playerInput.actions.FindAction("Zoom").performed += OnZoom;

        _playerInput.actions.FindAction("Move").performed += OnMove;
        _playerInput.actions.FindAction("Move").canceled += OnMove;

        _playerInput.actions.FindAction("MoveUpDown").performed += OnMoveUpDown;
        _playerInput.actions.FindAction("MoveUpDown").canceled += OnMoveUpDown;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Camera").performed -= OnCamera;
        _playerInput.actions.FindAction("Camera").canceled -= OnCamera;

        _playerInput.actions.FindAction("Zoom").performed -= OnZoom;

        _playerInput.actions.FindAction("Move").performed -= OnMove;
        _playerInput.actions.FindAction("Move").canceled -= OnMove;

        _playerInput.actions.FindAction("MoveUpDown").performed -= OnMoveUpDown;
        _playerInput.actions.FindAction("MoveUpDown").canceled -= OnMoveUpDown;
    }

    #endregion

    #region Inputs

    /// <summary>
    /// Handles camera rotation input
    /// </summary>
    /// <param name="context">input to read</param>
    private void OnCamera(InputAction.CallbackContext context)
    {
        _cameraInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles camera zoom input
    /// </summary>
    /// <param name="context">input to read</param>
    private void OnZoom(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<Vector2>().y;

        _targetZoom += scroll * 0.2f;
        _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
    }

    /// <summary>
    /// Handles camera move input when focussing on a target
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _isMoving = _moveInput != Vector2.zero;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveUpDown(InputAction.CallbackContext context)
    {
        _upDownInput = context.ReadValue<float>();
    }

    #endregion

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds a new target for the camera to look at
    /// </summary>
    /// <param name="newTarget"></param>
    public void AddTarget(Transform newTarget)
    {
        if (newTarget == null || _lookAtTarget == newTarget)
            return;

        _lookAtTarget = newTarget;
        _targetPin = _lookAtTarget.GetChild(0);

        Vector3 dir = (transform.position - _lookAtTarget.position).normalized;

        float scaledOffset = _lookAtDefaultOffset * _lookAtTarget.localScale.magnitude;

        _offsetToTarget = _lookAtTarget.InverseTransformPoint(_lookAtTarget.position + dir * scaledOffset);
    }

    /// <summary>
    /// Removes the current target the camera is looking at
    /// </summary>
    public void RemoveTarget()
    {
        if (_lookAtTarget != null && _isParented)
        {
            transform.SetParent(null);
            _isParented = false;
        }

        _lookAtTarget = null;
        _targetPin = null;
    }


    private void Update()
    {
        _currentZoom = _camera.transform.localPosition.z;
        _currentFOV = _camera.fieldOfView;

        HandleZoom();

        if (_lookAtTarget == null)
        {
            RotateCamera();

            FreeFly();
        }
        else
        {
            if (!_isParented && !_isMoving)
            {
                MoveTowardsTarget();
            }

            if (_isMoving)
            {
                RotateCameraAroundTarget();
            }
        }

        _cameraInput = Vector2.zero;
    }

    /// <summary>
    /// Late Update is used to ensure the camera movement is applied after all other updates.
    /// Preventing jitter when focussing on a target while it is moving.
    /// </summary>
    void LateUpdate()
    {
        if (!_isMoving && _lookAtTarget != null)
        {
            FocusOnPinConstantSpeed();
        }
    }


    /// <summary>
    /// Handles camera rotation based on player input
    /// </summary>
    private void RotateCamera()
    {
        _yRotation -= _cameraInput.y * _rotationSpeed * Time.deltaTime;
        _xRotation += _cameraInput.x * _rotationSpeed * Time.deltaTime;

        _yRotation = Mathf.Clamp(_yRotation, _minYClamp, _maxYClamp);

        transform.localRotation = Quaternion.Euler(0, _xRotation, 0);

        _camera.transform.localRotation = Quaternion.Euler(_yRotation, 0, 0);

        _orientation.forward = transform.forward;
    }

    /// <summary>
    /// Handles camera zooming
    /// the fov changes based on the zoom level
    /// </summary>
    private void HandleZoom()
    {
        Vector3 pos = _camera.transform.localPosition;
        pos.z = Mathf.MoveTowards(pos.z, _targetZoom, _zoomSpeed * Time.deltaTime);
        _camera.transform.localPosition = pos;

        float t = Mathf.InverseLerp(_minZoom, _maxZoom, _camera.transform.localPosition.z);
        _camera.fieldOfView = Mathf.Lerp(_zoomedOutFOV, _zoomedInFOV, t);
    }

    /// <summary>
    /// Moves the camera towards the target it is looking at and parents it when close enough
    /// </summary>
    private void MoveTowardsTarget()
    {
        if (_lookAtTarget == null) return;

        Vector3 targetWorldPos = _lookAtTarget.TransformPoint(_offsetToTarget);

        transform.position = Vector3.Lerp(transform.position, targetWorldPos, Time.deltaTime * _moveSpeed);

        _distanceFromTarget = Vector3.Distance(transform.position, _lookAtTarget.position);

        if (!_isParented && _distanceFromTarget <= _parentDistance)
        {
            ParentToTarget();
        }

        _camera.transform.LookAt(_lookAtTarget.position);
    }

    /// <summary>
    /// Parents the camera to the target it is looking at
    /// </summary>
    private void ParentToTarget()
    {
        if (_lookAtTarget == null) return;

        transform.SetParent(_lookAtTarget);

        transform.localPosition = _lookAtTarget.InverseTransformPoint(transform.position);
        transform.LookAt(_lookAtTarget.position);

        _isParented = true;
    }

    /// <summary>
    /// Rotates the camera around the target it is looking at based on player input
    /// </summary>
    private void RotateCameraAroundTarget()
    {
        if (_lookAtTarget == null)
            return;

        float xAxis = -_moveInput.x * _rotationSpeed * Time.deltaTime * 10;
        float yAxis = _moveInput.y * _rotationSpeed * Time.deltaTime * 10;

        transform.RotateAround(_lookAtTarget.position, transform.up, xAxis);
        transform.RotateAround(_lookAtTarget.position, transform.right, yAxis);
    }

    /// <summary>
    /// Focuses the camera on a pin of the target at a constant speed around the target itself
    /// </summary>
    private void FocusOnPinConstantSpeed()
    {
        if (_lookAtTarget == null || _lookAtTarget.childCount == 0) return;

        // Calculate directions relative to the center of the target
        Vector3 currentDir = (transform.position - _lookAtTarget.position).normalized;
        Vector3 targetDir = (_targetPin.position - _lookAtTarget.position).normalized;

        // Determine the angle between the current and target directions
        float angleBetween = Vector3.Angle(currentDir, targetDir);

        // If close to the angle stop to prevent jitter
        if (angleBetween < 0.01f) return;

        // Calculate how much to move this per step
        float moveStep = _focusSmoothSpeed * 100 * Time.deltaTime;

        // Devide the steps to get a linear value to move across the angle
        float t = Mathf.Min(moveStep / angleBetween, 1.0f);

        // Use slerp to smoothly interpolate between directions and maintain radius around the target
        float currentRadius = Vector3.Distance(transform.position, _lookAtTarget.position);
        Vector3 newRelativePos = Vector3.Slerp(currentDir, targetDir, t) * currentRadius;

        transform.position = _lookAtTarget.position + newRelativePos;
        transform.LookAt(_lookAtTarget.position);
    }

    private void FreeFly()
    {
        if (_moveInput == Vector2.zero && _upDownInput == 0)
        {
            _currentFlySpeed = _minFlySpeed;
            return;
        }

        Vector3 upDownDirection = _camera.transform.up * _upDownInput;
        transform.position += upDownDirection * _currentFlySpeed * Time.deltaTime;

        Vector3 moveDirection = _camera.transform.forward * _moveInput.y + _camera.transform.right * _moveInput.x;

        if (Mathf.Approximately(moveDirection.magnitude, _previousMoveDirection.magnitude))
        {
            _currentFlySpeed = Mathf.MoveTowards(_currentFlySpeed, _moveSpeed * _flySpeedMultiplier, _flySpeedChangeRate * Time.deltaTime);
            _currentFlySpeed = Mathf.Clamp(_currentFlySpeed, _minFlySpeed, _maxFlySpeed);
        }
        else
        {
            _currentFlySpeed = _minFlySpeed;
        }

        transform.position += moveDirection * _currentFlySpeed * Time.deltaTime;

        _previousMoveDirection = moveDirection;
    }
}