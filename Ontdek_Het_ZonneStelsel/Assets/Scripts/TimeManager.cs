using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private TMPro.TextMeshProUGUI _timeScaleText;
    [SerializeField] private RotateObject[] _rotateObjects;
    [SerializeField] private int[] _Multiply = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

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
        _timeScaleText.text = _Multiply[0] + "x";
    }

    public void ChangeTimeScale(int index)
    {
        foreach (RotateObject ro in _rotateObjects)
        {
            if (ro == null) continue;
            ro.multiplier = _Multiply[index];
        }
        _timeScaleText.text = _Multiply[index] + "x";
    }
}