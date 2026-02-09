using UnityEngine;

public class PlanetInfo : MonoBehaviour
{
    [Tooltip("Planet name")]
    [SerializeField] private string _planetName;
    public string PlanetName => _planetName;

    [Tooltip("Time to orbit the sun (in days)")]
    [SerializeField] private float _orbitalPeriodDays;
    public float OrbitalPeriodDays => _orbitalPeriodDays;

    [Tooltip("Rotation period around its axis (in days)")]
    [SerializeField] private float _rotationPeriodDays;
    public float RotationPeriodDays => _rotationPeriodDays;

    [Tooltip("Simulation distance")]
    [SerializeField] private float _simulationDistance;
    public float SimulationDistance => _simulationDistance;

    [Tooltip("Distance to the sun (millions of km)")]
    [SerializeField] private float _distanceToSunMillionsKm;
    public float DistanceToSunMillionsKm => _distanceToSunMillionsKm;

    [Tooltip("Distance to Earth (millions of km)")]
    [SerializeField] private float _distanceToEarthMillionsKm;
    public float DistanceToEarthMillionsKm => _distanceToEarthMillionsKm;

    [Tooltip("Mass (10^24 kilograms)")]
    [SerializeField] private float _mass10e24Kg;
    public float Mass10e24Kg => _mass10e24Kg;

    [Tooltip("Simulation scale (relative to real size)")]
    [SerializeField] private float _simulationScale;
    public float SimulationScale => _simulationScale;

    [Tooltip("Actual size")]
    [SerializeField] private float _actualSize;
    public float ActualSize => _actualSize;

    [Tooltip("Planet view point (empty GameObject)")]
    [SerializeField] private GameObject _viewPoint;
    public GameObject ViewPoint => _viewPoint;

    [Tooltip("Planet scene name")]
    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;
}
