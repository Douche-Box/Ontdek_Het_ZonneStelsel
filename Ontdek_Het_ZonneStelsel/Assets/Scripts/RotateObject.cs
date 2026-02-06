using System.Diagnostics;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public enum RotateType
    {
        AroundSelf,
        AroundSun
    }

    [Header("Rotate Settings")]
    [SerializeField] private RotateType rotateType;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [Tooltip("Ruimte waarin de as geïnterpreteerd wordt (Local = object-as, World = wereld-as)")]
    [SerializeField] private Space axisSpace = Space.Self;

    [Tooltip("Gameplay multiplier bovenop realistische snelheid")]
    [SerializeField] private float rotationSpeed = 1000f;

    [Header("References")]
    [SerializeField] private Transform orbitObject;
    [SerializeField] private PlaneetInformatie planeetInfo;

    [Header("Overrides (optioneel)")]
    [SerializeField] private bool useManualSpeed = false;
    [SerializeField] private float manualSpeed = 10f;
    public float multiplier = 1;

    void Update()
    {
        float speed = GetRotationSpeed();

        switch (rotateType)
        {
            case RotateType.AroundSelf:
                transform.Rotate(rotationAxis, speed * multiplier * Time.deltaTime, axisSpace);
                break;

            case RotateType.AroundSun:
                if (orbitObject == null) return;

                // RotateAround expects the axis in world space. If the user chose local space,
                // convert the local axis to world space first.
                Vector3 worldAxis = axisSpace == Space.Self ? transform.TransformDirection(rotationAxis) : rotationAxis;

                transform.RotateAround(
                    orbitObject.position,
                    worldAxis,
                    speed * multiplier * Time.deltaTime
                );
                break;
        }
    }

    private float GetRotationSpeed()
    {
        if (useManualSpeed || planeetInfo == null)
            return manualSpeed;

        switch (rotateType)
        {
            case RotateType.AroundSelf:
                // uren -> graden per seconde
                return (1f / 360f / planeetInfo.asDraaiSnelheid) * rotationSpeed;

            // 1:25 = 0.04 (draaisnelheid)
            // 1:59 = 0.0167 (snelheid)
            // 1:planeetInfo.asDraaiSnelheid
            // nr2 is langzamer dan nr1

            case RotateType.AroundSun:
                // dagen -> graden per seconde
                return (1f / 360f / planeetInfo.omZonDraaiTijd) * rotationSpeed;

                // 1:25 = 0.04 (draaisnelheid)
                // 1:1 = 1 (snelheid)
                // 1:planeetInfo.omZonDraaiTijd
                // nr2 is sneller dan nr1
        }

        return 0f;
    }
}
