using UnityEngine;

public class TargetObject : MonoBehaviour
{
    [Header("Info")]
    #region info
    [SerializeField] private string objectName;
    public string ObjectName => objectName;
    [SerializeField] private bool isHighlighted;
    public bool IsHighlighted => isHighlighted;
    [SerializeField] private bool isSelected;
    public bool IsSelected => isSelected;
    [SerializeField] private string info;
    public string Info => info;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
