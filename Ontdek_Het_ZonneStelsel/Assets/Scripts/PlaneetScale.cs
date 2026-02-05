using UnityEngine;
using UnityEngine.UIElements;

public class PlaneetScale : MonoBehaviour
{
    public PlaneetInformatie planeetInformatie;
    [SerializeField] private bool useSimulationScale;
    public int schaal = 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        schaal = 100;
        UpdateScale();
    }
    public void UpdateScale()
    {
        if (useSimulationScale == false)
        {
            transform.localScale = Vector3.one * planeetInformatie.wareGrootte / schaal;
        }
        else
        {
            transform.localScale = Vector3.one * planeetInformatie.simulatieGrootte;
        }
    }
    public void SwitchDistanceMode(bool simulation)
    {
        useSimulationScale = simulation;
        UpdateScale();
    }
}
