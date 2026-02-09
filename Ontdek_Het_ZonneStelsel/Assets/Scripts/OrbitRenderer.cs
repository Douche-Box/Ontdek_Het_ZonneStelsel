using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _centerObject; // Sun
    [SerializeField] private PlanetInfo _planeetInfo;

    [Header("Visual Settings")]
    [SerializeField] private int _segments = 100;
    [SerializeField] private bool _useSimulationDistance = true;
    [SerializeField] private float _simulationLineWidth = 0.75f;
    [SerializeField] private float _realisticLineWidth = 0.001f;
    [SerializeField] private float _scale = 100f;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
    }

    void Start()
    {
        DrawOrbit();
    }

    /// <summary>
    /// Draws the orbit of the planet based on either the simulation distance or the realistic distance depending on the current mode.
    /// The orbit is rendered as a circle around the center object (the sun) using a LineRenderer component. 
    /// The radius of the orbit is determined by the selected distance mode, and the line width is adjusted accordingly for better visibility in each mode.
    /// </summary>
    public void DrawOrbit()
    {
        if (_planeetInfo == null || _centerObject == null) return;

        float radius = _useSimulationDistance
            ? _planeetInfo.SimulationDistance
            : _planeetInfo.DistanceToSunMillionsKm;

        // uses scale factor to make the orbits visible in the scene when using realistic distances, while keeping them accurate when using simulation distances.
        float effectiveRadius = _useSimulationDistance 
            ? radius 
            : radius * _scale;
        
        float lineWidth = _useSimulationDistance
            ? _simulationLineWidth
            : _realisticLineWidth;

        lineRenderer.positionCount = _segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;


        for (int i = 0; i < _segments; i++)
        {
            float angle = (float)i / _segments * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * effectiveRadius;
            float z = Mathf.Sin(angle) * effectiveRadius;

            Vector3 pos = _centerObject.position + new Vector3(x, 0f, z);
            lineRenderer.SetPosition(i, pos);
        }
    }

    public void SwitchDistanceMode(bool simulation)
    {
        _useSimulationDistance = simulation;
        DrawOrbit();
    }
}
