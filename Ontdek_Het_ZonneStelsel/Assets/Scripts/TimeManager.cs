using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private RotateObject[] rotateObjects;
    [SerializeField] private int[] VermenigvuldegingsFactoren = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

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
    }

    public void ChangeTimeScale(int index)
    {
        foreach (RotateObject ro in rotateObjects)
        {
            if (ro == null) continue;
            ro.multiplier = VermenigvuldegingsFactoren[index];
        }
    }
}