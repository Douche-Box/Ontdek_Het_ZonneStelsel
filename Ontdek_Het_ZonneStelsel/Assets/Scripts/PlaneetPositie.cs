using UnityEngine;

public class PlaneetPositie : MonoBehaviour
{
    [SerializeField] private PlaneetInformatie planeetInfo;
    [SerializeField] private bool useSimulationDistance = true;

    private float originalY;
    private float originalZ;

    private void Awake()
    {
        // Bewaar Y en Z, die veranderen we nooit
        originalY = transform.position.y;
        originalZ = transform.position.z;
    }

    private void Start()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (planeetInfo == null) return;

        float x = useSimulationDistance
            ? planeetInfo.simulatieAfstand
            : planeetInfo.afstandTotZon;

        transform.position = new Vector3(x, originalY, originalZ);
    }

    public void SwitchDistanceMode(bool simulation)
    {
        useSimulationDistance = simulation;
        UpdatePosition();
    }
}
