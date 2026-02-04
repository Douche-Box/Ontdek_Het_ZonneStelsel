using UnityEngine;

public class PlaneetPositie : MonoBehaviour
{
    [SerializeField] private PlaneetInformatie planeetInfo;
    [SerializeField] private bool useSimulationDistance = true;

    private Vector3 orbitDirection;

    private void Awake()
    {
        // Sla originele richting op (t.o.v. zon)
        orbitDirection = (transform.position - Vector3.zero).normalized;
    }

    private void Start()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (planeetInfo == null) return;

        float distance = useSimulationDistance
            ? planeetInfo.simulatieAfstand
            : planeetInfo.afstandTotZon;

        transform.localPosition = orbitDirection * distance;
    }

    public void SwitchDistanceMode(bool simulation)
    {
        useSimulationDistance = simulation;
        UpdatePosition();
    }
}
