using UnityEngine;

public class PlanetSimulationChange : MonoBehaviour
{
    public static PlanetSimulationChange Instance { get; private set; }

    [SerializeField] private OrbitRenderer[] _orbitRenderer;
    [SerializeField] private PlanetPosition[] _distance;
    [SerializeField] private PlanetScale[] _planetScale;
    [SerializeField] private bool _normalMode = false;

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
        Debug.Log("Start in " + (_normalMode ? "simulatie" : "realistische") + " modus.");
    }

    /// <summary>
    /// Toggles between simulation mode and realistic mode for all planets.
    /// When toggled, it updates the distance mode for all orbit renderers, distance displays.
    /// </summary>
    public void ToggleModeSimulationMode()
    {
        _normalMode = !_normalMode;

        foreach (var orbit in _orbitRenderer)
        {
            orbit.SwitchDistanceMode(_normalMode);
        }
        foreach (var afstand in _distance)
        {
            afstand.SwitchDistanceMode(_normalMode);
        }
        foreach (var scale in _planetScale)
        {
            scale.SwitchDistanceMode(_normalMode);
        }
    }
}