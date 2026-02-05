using UnityEngine;

public class PlaneetVersnellen : MonoBehaviour
{
    [SerializeField] private RotateObject[] rotateObjects;
    [SerializeField] private int VermenigvuldegingsFactor = 10;
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private float minMultiplier = 0.01f;
    [SerializeField] private float maxMultiplier = 100f;

    public void VersnelTijd()
    {
        if (rotateObjects == null) return;

        multiplier *= VermenigvuldegingsFactor;
        float min = Mathf.Min(minMultiplier, maxMultiplier);
        float max = Mathf.Max(minMultiplier, maxMultiplier);
        multiplier = Mathf.Clamp(multiplier, min, max);

        foreach (RotateObject ro in rotateObjects)
        {
            if (ro == null) continue;
            ro.multiplier = multiplier;
        }
    }
    public void VersloomTijd()
    {
        if (rotateObjects == null) return;

        multiplier /= VermenigvuldegingsFactor;
        float min2 = Mathf.Min(minMultiplier, maxMultiplier);
        float max2 = Mathf.Max(minMultiplier, maxMultiplier);
        multiplier = Mathf.Clamp(multiplier, min2, max2);

        foreach (RotateObject ro in rotateObjects)
        {
            if (ro == null) continue;
            ro.multiplier = multiplier;
        }
    }
}