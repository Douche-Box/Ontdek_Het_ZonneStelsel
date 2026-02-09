using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum RotateType
    {
        AroundSelf,
        AroundSun
    }

    [Header("Rotate Settings")]
    [SerializeField] private RotateType _rotateType;
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;

    [Tooltip("Ruimte waarin de as geïnterpreteerd wordt (Local = object-as, World = wereld-as)")]
    [SerializeField] private Space _axisSpace = Space.Self;

    [Tooltip("Gameplay multiplier bovenop realistische snelheid")]
    [SerializeField] private float _rotationSpeed = 1000f;

    [Header("References")]
    [SerializeField] private Transform _orbitObject;
    [SerializeField] private PlanetInfo _planetInfo;

    [Header("Overrides (optioneel)")]
    [SerializeField] private bool _useManualSpeed = false;
    [SerializeField] private float _manualSpeed = 10f;
    public float multiplier = 1;

/// <summary>
/// Rotates the object based on the selected rotation type (around itself or around the sun).
/// </summary>
    void Update()
    {
        float speed = GetRotationSpeed();

        switch (_rotateType)
        {
            case RotateType.AroundSelf:
                transform.Rotate(_rotationAxis, speed * multiplier * Time.deltaTime, _axisSpace);
                break;

            case RotateType.AroundSun:
                if (_orbitObject == null) return;

                // RotateAround expects the axis in world space. If the user chose local space,
                // convert the local axis to world space first.
                Vector3 worldAxis = _axisSpace == Space.Self ? transform.TransformDirection(_rotationAxis) : _rotationAxis;

                transform.RotateAround(_orbitObject.position, worldAxis, speed * multiplier * Time.deltaTime);
                break;
        }
    }

/// <summary>
/// Calculates the rotation speed based on the selected mode (manual or automatic) and the rotation type.
/// </summary>
    private float GetRotationSpeed()
    {
        if (_useManualSpeed || _planetInfo == null)
            return _manualSpeed;

        switch (_rotateType)
        {
            case RotateType.AroundSelf:
                // Days -> degrees per second
                return 1f / 360f / _planetInfo.RotationPeriodDays * _rotationSpeed;

            // 1:25 = 0.04 (draaisnelheid)
            // 1:59 = 0.0167 (snelheid)
            // 1:planetInfo.RotationPeriodDays
            // nr2 is langzamer dan nr1

            case RotateType.AroundSun:
                // Days -> degrees per second
                return 1f / 360f / _planetInfo.OrbitalPeriodDays * _rotationSpeed;

                // 1:25 = 0.04 (draaisnelheid)
                // 1:1 = 1 (snelheid)
                // 1:planetInfo.OrbitalPeriodDays
                // nr2 is sneller dan nr1
        }

        return 0f;
    }
}
