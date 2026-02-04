using UnityEngine;

public class PlaneetAfstandVerandering : MonoBehaviour
{
    [SerializeField] private BaanVisualisatie[] baanVisualisatie;
    [SerializeField] private PlaneetPositie[] afstands;
    [SerializeField] private PlaneetScale[] planeetScale;
    [SerializeField] private bool normalMode = false;


    private void Start()
    {
        ToggleModeSimulationMode();
    }
    public void ToggleModeSimulationMode()
    {
        normalMode = !normalMode;

        foreach (var orbit in baanVisualisatie)
        {
            orbit.SwitchDistanceMode(normalMode);
        }
        foreach (var afstand in afstands)
        {
            afstand.SwitchDistanceMode(normalMode);
        }
        foreach (var scale in planeetScale)
        {
            scale.SwitchDistanceMode(normalMode);
        }   
    }
}