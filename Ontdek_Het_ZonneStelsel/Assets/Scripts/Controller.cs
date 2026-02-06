using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject lastHighlightedObject;
    [SerializeField] private GameObject currentSelectedObject;
    public CameraController cameraController;
    [SerializeField] private PlayerInput playerInput;
    public TargetObject targetObject;

    public bool highlight = false;
    public Material highlightMaterial;
    public Material[] originalMaterials;
    Material[] allMaterials;




    private GameObject currentHighlightedObject;

    private void OnEnable()
    {
        playerInput.actions.FindAction("Click").started += OnClick;
    }
    private void OnDisable()
    {
        playerInput.actions.FindAction("Click").started -= OnClick;
    }
    void Update()
    {
        HandleHighlight();
        /*
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            currentSelectedObject = hit.transform.gameObject;
            currentSelectedObject.TryGetComponent(out Renderer renderer);
            originalMaterials = new Material[renderer.materials.Length + 1];
            allMaterials = originalMaterials;
            allMaterials[allMaterials.Length -1] = highlightMaterial;

            var mats = renderer.materials;
            allMaterials = new Material[mats.Length + 1];
            mats.CopyTo(allMaterials, 0);
            allMaterials[allMaterials.Length - 1] = highlightMaterial;
            renderer.materials = allMaterials;




            highlight = true;
            Debug.Log("highlight true");
            currentSelectedObject = hit.transform.gameObject;
            //UI_manager.Instance.ShowPopup(currentObject.ObjectName, currentObject.Info);
        }
        else
        {
            highlight = false;
            clearHighlight();
        }
        */
    }
    public void AddTarget(Transform newTarget)
    {
        if (currentSelectedObject != newTarget.gameObject)
        {
            Debug.Log("addtarget");
            

            
        }
        else
        {
            currentSelectedObject.TryGetComponent(out Renderer renderer);
            originalMaterials = new Material[renderer.materials.Length - 1];
            currentSelectedObject = null;
        }
    }
    public void RemoveTarget()
    {

    }

    public void SelectObject()
    {

    }
    public void Move()
    {

    }
    public void clearHighlight()
    {
        Debug.Log("clearnHIghliight");
        currentSelectedObject.TryGetComponent(out Renderer renderer);
        originalMaterials = new Material[renderer.materials.Length - 1];
        currentSelectedObject = null;
    }
    public void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.Log("cliCK");
            AddTarget(hit.transform);
        }
        else
        {
            Debug.Log("other click");
            clearHighlight();
        }
    }

    public void HandleHighlight()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            ClearHighlight();
            return;
        }

        GameObject hitObject = hit.collider.gameObject;

        // Do nothing if already highlighted
        if (hitObject == currentHighlightedObject)
            return;

        ClearHighlight();
        HighlightObject(hitObject);
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
        originalMaterials = null;
    }
 
    private void HighlightObject(GameObject obj)
    {
        if (!obj.TryGetComponent(out Renderer renderer))
            return;

        currentHighlightedObject = obj;
        originalMaterials = renderer.materials;

        Material[] newMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(newMaterials, 0);
        newMaterials[newMaterials.Length - 1] = highlightMaterial;

        renderer.materials = newMaterials;
    }
}
