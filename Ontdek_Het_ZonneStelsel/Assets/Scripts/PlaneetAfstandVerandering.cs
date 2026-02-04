using UnityEngine;

public class PlaneetAfstandVerandering : MonoBehaviour
{
    [SerializeField] private BaanVisualisatie[] baanVisualisatie;
    [SerializeField] private PlaneetPositie[] afstands;
    [SerializeField] private bool simulationMode = false;

    private void Start()
    {
        ToggleModeSimulationMode();
    }
    public void ToggleModeSimulationMode()
    {
        simulationMode = !simulationMode;

        foreach (var orbit in baanVisualisatie)
        {
            orbit.SwitchDistanceMode(simulationMode);
        }
        foreach (var afstand in afstands)
        {
            afstand.SwitchDistanceMode(simulationMode);
        }
    }
}