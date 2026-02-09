using UnityEngine;

public class PlanetPosition : MonoBehaviour
{
    [SerializeField] private PlanetInfo _planeetInfo;
    [SerializeField] private bool _useSimulationDistance = true;
    [SerializeField] private float _scale = 10f;
    private Vector3 _orbitDirection;

    private void Awake()
    {
        // saves the origional direction from the sun to the planet, which is used to calculate the position of the planet in both simulation and realistic modes.
        _orbitDirection = (transform.position - Vector3.zero).normalized;
    }

    private void Start()
    {
        UpdatePosition();
    }
    
/// <summary>
/// Updates the position of the planet based on the selected distance mode (simulation or realistic).
/// </summary>
    public void UpdatePosition()
    {
        if (_planeetInfo == null) return;

        float distance = _useSimulationDistance
            ? _planeetInfo.SimulationDistance
            : _planeetInfo.DistanceToSunMillionsKm;

        if (_useSimulationDistance)
        {
            transform.localPosition = _orbitDirection * distance;   
        }
        else
        {
            transform.localPosition = _orbitDirection * distance / _scale;
        }
    }

    public void SwitchDistanceMode(bool simulation)
    {
        _useSimulationDistance = simulation;
        UpdatePosition();
    }
}
