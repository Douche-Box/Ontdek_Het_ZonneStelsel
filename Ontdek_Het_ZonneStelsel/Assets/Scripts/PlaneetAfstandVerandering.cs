using UnityEngine;

public class PlaneetAfstandVerandering : MonoBehaviour
{
    public static PlaneetAfstandVerandering Instance { get; private set; }

    [SerializeField] private BaanVisualisatie[] baanVisualisatie;
    [SerializeField] private PlaneetPositie[] afstands;
    [SerializeField] private PlaneetScale[] planeetScale;
    [SerializeField] private bool normalMode = false;

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

    private void Start()
    {
        ToggleModeSimulationMode();
        Debug.Log("Start in " + (normalMode ? "simulatie" : "realistische") + " modus.");
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