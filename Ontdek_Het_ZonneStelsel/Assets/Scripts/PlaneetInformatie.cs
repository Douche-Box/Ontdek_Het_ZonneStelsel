
using UnityEngine;

public class PlaneetInformatie : MonoBehaviour 
{
    [Tooltip("Naam van de planeet")]
    [SerializeField] private string _planeetNaam;
    public string planeetNaam => _planeetNaam;

    [Tooltip("hoelang duurt een draai om de zon")]
    [SerializeField] private float _omZonDraaiTijd;
    public float omZonDraaiTijd => _omZonDraaiTijd; // in dagen

    [Tooltip("hoelang duurt een draai om zijn as")]
    [SerializeField] private float _asDraaiSnelheid;
    public float asDraaiSnelheid => _asDraaiSnelheid; // in uren

    [Tooltip("Afstand tot de zon in miljoenen kilometers")]
    [SerializeField] private float _afstandTotZon;
    public float afstandTotZon => _afstandTotZon;
    
    [Tooltip("Massa in 10^24 kilogram")]
    [SerializeField] private float _massa;
    public float massa => _massa;

    [Tooltip("Grootte verkleint")]
    [SerializeField] private float _grootteVerkleining;
    public float grootteVerkleining => _grootteVerkleining;

    [Tooltip("Ware grootte")]
    [SerializeField] private float _wareGrootte;
    public float wareGrootte => _wareGrootte;

    [Tooltip("Aankijk punt planeet (empty object)")]
    [SerializeField] private GameObject viewPoint;
    public GameObject ViewPoint => viewPoint;

    [Tooltip("Planeet scene")]
    [SerializeField] private string _sceneNaam;
    public string sceneNaam => _sceneNaam;
}
