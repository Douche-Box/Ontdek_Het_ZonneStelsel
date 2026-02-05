using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BaanVisualisatie : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform centerObject; // Zon
    [SerializeField] private PlaneetInformatie planeetInfo;

    [Header("Visual Settings")]
    [SerializeField] private int segments = 100;
    [SerializeField] private bool useSimulationDistance = true;
    [SerializeField] private float simulationLineWidth = 0.75f;
    [SerializeField] private float realisticLineWidth = 0.001f;
    [SerializeField] private float scale = 100f;


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

    public void DrawOrbit()
    {
        if (planeetInfo == null || centerObject == null) return;

        float radius = useSimulationDistance
            ? planeetInfo.simulatieAfstand
            : planeetInfo.afstandTotZon;

        // Gebruikt de scale alleen wanneer we de realistische afstand tonen.
        float effectiveRadius = useSimulationDistance 
            ? radius 
            : radius * scale;
        
        float lineWidth = useSimulationDistance
            ? simulationLineWidth
            : realisticLineWidth;

        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;


        for (int i = 0; i < segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * effectiveRadius;
            float z = Mathf.Sin(angle) * effectiveRadius;

            Vector3 pos = centerObject.position + new Vector3(x, 0f, z);
            lineRenderer.SetPosition(i, pos);
        }
    }

    public void SwitchDistanceMode(bool simulation)
    {
        useSimulationDistance = simulation;
        DrawOrbit();
    }
}
