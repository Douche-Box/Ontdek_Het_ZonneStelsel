using UnityEngine;

public class PlanetScale : MonoBehaviour
{
    [SerializeField] private PlanetInfo _planetInfo;
    public PlanetInfo PlanetInfo => _planetInfo;
    [SerializeField] private bool _useSimulationScale;
    public int scale;

    void Start()
    {
        scale = 100;
        UpdateScale();
    }

    public void UpdateScale()
    {
        if (_useSimulationScale == false)
        {
            transform.localScale = Vector3.one * _planetInfo.ActualSize / scale;
        }
        else
        {
            transform.localScale = Vector3.one * _planetInfo.SimulationScale;
        }
    }

    public void SwitchDistanceMode(bool simulation)
    {
        _useSimulationScale = simulation;
        UpdateScale();
    }
}
