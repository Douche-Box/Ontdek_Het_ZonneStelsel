using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    #region Refrences

    [SerializeField] private Camera _camera;
    public Camera Camera => _camera;

    [SerializeField] private PlayerInput _playerInput;
    public PlayerInput PlayerInput => _playerInput;

    [SerializeField] private Transform _orientation;

    #endregion

    [Header("Inputs")]
    #region Inputs

    [SerializeField] private Vector2 _cameraInput;
    [SerializeField] private float _yRotation;
    [SerializeField] private float _xRotation;

    [SerializeField] private float _zoomInput;

    #endregion

    [Header("Settings")]
    #region Settings

    [Header("Camera")]
    #region Camera

    [SerializeField] private float _rotationSpeed = 2.0f;

    [SerializeField] private float _minYClamp = -89.9f;
    [SerializeField] private float _maxYClamp = 89.9f;

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

    #endregion

    #region Subscriptions

    private void OnEnable()
    {
        _playerInput.actions.FindAction("Camera").performed += OnCamera;
        _playerInput.actions.FindAction("Camera").canceled += OnCamera;

        _playerInput.actions.FindAction("Zoom").performed += OnZoom;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Camera").performed -= OnCamera;
        _playerInput.actions.FindAction("Camera").canceled -= OnCamera;

        _playerInput.actions.FindAction("Zoom").performed -= OnZoom;
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


    #endregion

    private void Update()
    {
        _currentZoom = _camera.transform.localPosition.z;
        _currentFOV = _camera.fieldOfView;

        RotateCamera();
        HandleZoom();

        _cameraInput = Vector2.zero;
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
}
