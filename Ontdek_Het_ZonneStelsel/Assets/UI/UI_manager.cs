using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    public static UI_manager Instance { get; private set; }

    public int volume;
    public GameObject pauseMenu;
    public PlayerInput playerInput;
    public PlaneetInformatie planeetInformatie;
    public PlaneetAfstandVerandering planeetAfstandVerandering;
    public GameObject landenOpPlaneet;
    public GameObject verlaatPlaneet;
    public bool opPlaneet = false;
    public bool planeetIsSelected = false;

    public string selectedPlaneet;

    [Header("Settings Menu")]
    public Slider textSpeedSlider;
    public TMP_InputField textSpeedInputField;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        float savedTextSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.5f);
        textSpeedSlider.value = savedTextSpeed;
        textSpeedInputField.text = savedTextSpeed.ToString("F2");
        TextWriter.Instance.TimeToWaitForNextChar = savedTextSpeed;
    }

    public void OnEnable()
    {
        playerInput.actions.FindAction("PauseMenu").performed += OnPause;

        if (textSpeedInputField != null)
            textSpeedInputField.onEndEdit.AddListener(OnTextSpeedValueChanged);
        if (textSpeedSlider != null)
            textSpeedSlider.onValueChanged.AddListener(OnTextSpeedValueChanged);
    }

    private void OnTextSpeedValueChanged(float newTextSpeed)
    {
        float textSpeed = newTextSpeed;
        textSpeedInputField.text = textSpeed.ToString("F2");
        TextWriter.Instance.TimeToWaitForNextChar = textSpeed;

        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
    }

    private void OnTextSpeedValueChanged(string newTextSpeed)
    {
        float textSpeed;
        float.TryParse(newTextSpeed, out textSpeed);
        textSpeedSlider.value = textSpeed;
        textSpeedInputField.text = textSpeed.ToString("F2");
        TextWriter.Instance.TimeToWaitForNextChar = textSpeed;

        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (planeetIsSelected == true)
        {
            landenOpPlaneet.SetActive(!landenOpPlaneet.activeSelf);
        }
        if (opPlaneet == true)
        {
            verlaatPlaneet.SetActive(!verlaatPlaneet.activeSelf);
        }
    }

    public void Terug()
    {

    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void SchaalToggle()
    {
        planeetAfstandVerandering.ToggleModeSimulationMode();

    }
    public void LandOpPlaneet(string selectedPlanet)
    {
        SceneManager.LoadScene(selectedPlanet);
    }

    public void VerlaatPlaneet()
    {
        SceneManager.LoadScene("Zonnestelsel");
    }
}
