using UnityEngine;
using UnityEngine.UIElements;

public class PlaneetPositie : MonoBehaviour
{
    [SerializeField] private PlaneetInformatie planeetInfo;
    [SerializeField] private bool useSimulationDistance = true;
    [SerializeField] private float scale = 10f;
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

        if (useSimulationDistance)
        {
            transform.localPosition = orbitDirection * distance;   
        }
        else
        {
            transform.localPosition = orbitDirection * distance / scale;
        }
    }

    public void SwitchDistanceMode(bool simulation)
    {
        useSimulationDistance = simulation;
        UpdatePosition();
    }
}
