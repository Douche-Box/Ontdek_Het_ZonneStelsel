using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; private set; }

    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject lastHighlightedObject;
    private GameObject currentHighlightedObject;
    [SerializeField] private GameObject currentSelectedObject;
    [SerializeField] private PlayerInput playerInput;
    public TargetObject targetObject;

    public bool highlight = false;
    public Material highlightMaterial;
    public Material[] originalMaterials;
    public Material[] allMaterials;

    [SerializeField] private bool _isOnPlanet = false;
    public bool IsOnPlanet
    {
        get => _isOnPlanet;
        set => _isOnPlanet = value;
    }

    [SerializeField] private Ray _highlightRay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        playerInput.actions.FindAction("Click").started += OnClick;
    }
    private void OnDisable()
    {
        playerInput.actions.FindAction("Click").started -= OnClick;
    }

    public void GetCamera()
    {
        _camera = FindAnyObjectByType<Camera>();

        if (_camera == null)
        {
            Debug.LogError("Camera not found in the scene.");
        }
    }

    void Update()
    {
        HandleHighlight();

        if (_camera == null)
        {
            GetCamera();
        }
    }

    public void AddTarget(Transform newTarget)
    {
        if (currentSelectedObject == newTarget.gameObject && !IsOnPlanet)
        {
            RemoveTarget();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            currentSelectedObject = null;
            return;
        }

        currentSelectedObject = newTarget.gameObject;

        if (currentSelectedObject.TryGetComponent(out TargetObject targetObj))
        {
            if (CameraController.Instance != null)
                CameraController.Instance.AddTarget(newTarget);

            UI_manager.Instance.ShowPopup(targetObj.ObjectName, targetObj.Info);

            if (IsOnPlanet)
            {
                UI_manager.Instance.ShowLeaveButton();
            }
            else
            {
                UI_manager.Instance.ShowLandButton(targetObj.ObjectName);
            }
        }
    }

    public void RemoveTarget()
    {
        CameraController.Instance.RemoveTarget();
        UI_manager.Instance.HidePopup();
    }

    public void clearHighlight()
    {
        if (currentSelectedObject != null)
        {
            currentSelectedObject.TryGetComponent(out Renderer renderer);
            originalMaterials = new Material[renderer.materials.Length - 1];
            currentSelectedObject = null;
        }
    }

    /// <summary>
    /// Handles the click input to select or deselect objects in the scene.
    /// If the cursor is visible and unlocked, it uses the mouse position.
    /// Otherwise it casts a ray from the center of the screen. If an object is hit, it becomes the current selected object. 
    /// </summary>
    /// <param name="context"></param>
    public void OnClick(InputAction.CallbackContext context)
    {
        // If clicking UI, ignore interaction
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray;

        if (Cursor.visible && Cursor.lockState == CursorLockMode.None)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ray = _camera.ScreenPointToRay(mousePos);
        }
        else
        {
            ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            AddTarget(hit.transform);
        }
        else
        {
            clearHighlight();
        }
    }

    public void HandleHighlight()
    {
        if (_camera == null || !highlight)
        {
            ClearHighlight();
            return;
        }

        if (_isOnPlanet)
        {
            highlightMaterial.SetFloat("_Scale", 0.01f);

            Vector2 mousePos = Mouse.current.position.ReadValue();
            _highlightRay = _camera.ScreenPointToRay(mousePos);
        }
        else
        {
            highlightMaterial.SetFloat("_Scale", 1f);

            _highlightRay = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        }

        if (!Physics.Raycast(_highlightRay, out RaycastHit hit))
        {
            ClearHighlight();
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        if (hitObject == currentHighlightedObject)
            return;

        ClearHighlight();
        HighlightObject(hitObject);
    }

    public void ClearHighlight()
    {
        if (currentHighlightedObject == null)
            return;

        if (currentHighlightedObject.TryGetComponent(out Renderer renderer))
        {
            renderer.materials = originalMaterials;
        }

        currentHighlightedObject = null;
        originalMaterials = null;
    }

    private void HighlightObject(GameObject obj)
    {
        if (!obj.TryGetComponent(out Renderer renderer))
            return;

        currentHighlightedObject = obj;
        originalMaterials = renderer.materials;

        allMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(allMaterials, 0);
        allMaterials[allMaterials.Length - 1] = highlightMaterial;

        renderer.materials = allMaterials;
    }
}
