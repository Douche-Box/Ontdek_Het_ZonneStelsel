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
    public GameObject landenOpPlaneet;
    public GameObject verlaatPlaneet;
    public bool opPlaneet = false;
    public bool planeetIsSelected = false;

    public string selectedPlaneet;

    [SerializeField] private GameObject _popupPanel;

    [SerializeField] private GameObject _tabMenu;

    [Header("Settings Menu")]
    [SerializeField] private Slider _textSpeedSlider;
    [SerializeField] private TMP_InputField _textSpeedInputField;

    [SerializeField] private Slider _speedSlider;

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

    /// <summary>
    /// Initializes the settings menu with saved text speed from PlayerPrefs.
    /// </summary>
    private void Start()
    {
        float savedTextSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.5f);
        _textSpeedSlider.value = savedTextSpeed;
        _textSpeedInputField.text = savedTextSpeed.ToString("F2");
        TextWriter.Instance.CharactersPerSecond = savedTextSpeed;
    }

    #region Inputs

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("PauseMenu").performed += OnPause;

            playerInput.actions.FindAction("Tab").performed += OnTab;
        }


        if (_textSpeedInputField != null)
            _textSpeedInputField.onEndEdit.AddListener(OnTextSpeedInputField);

        if (_textSpeedSlider != null)
            _textSpeedSlider.onValueChanged.AddListener(OnTextSpeedSlider);

        if (_speedSlider != null)
            _speedSlider.onValueChanged.AddListener(OnPlanetSpeedSlider);
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("PauseMenu").performed -= OnPause;

            playerInput.actions.FindAction("Tab").performed -= OnTab;
        }


        if (_textSpeedInputField != null)
            _textSpeedInputField.onEndEdit.RemoveListener(OnTextSpeedInputField);

        if (_textSpeedSlider != null)
            _textSpeedSlider.onValueChanged.RemoveListener(OnTextSpeedSlider);

        if (_speedSlider != null)
            _speedSlider.onValueChanged.RemoveListener(OnPlanetSpeedSlider);
    }

    #endregion


    /// <summary>
    /// Called when the slider value changes and updates the input field and text writer speed. 
    /// Also saves the value to PlayerPrefs.
    /// Uses "F2" to format the float to 2 decimals.
    /// </summary>
    /// <param name="newTextSpeed"></param>
    private void OnTextSpeedSlider(float newTextSpeed)
    {
        float textSpeed = newTextSpeed;
        _textSpeedInputField.text = textSpeed.ToString("F2");
        TextWriter.Instance.CharactersPerSecond = textSpeed;

        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
    }

    /// <summary>
    /// Called when the input field value changes and updates the slider and text writer speed. 
    /// Also saves the value to PlayerPrefs.
    /// Uses "F2" to format the float to 2 decimals.
    /// </summary>
    /// <param name="newTextSpeed"></param>
    private void OnTextSpeedInputField(string newTextSpeed)
    {
        float textSpeed;
        float.TryParse(newTextSpeed, out textSpeed);
        _textSpeedSlider.value = textSpeed;
        _textSpeedInputField.text = textSpeed.ToString("F2");
        TextWriter.Instance.CharactersPerSecond = textSpeed;

        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
    }

    public void OnPlanetSpeedSlider(float newSpeed)
    {
        int index = (int)newSpeed;
        TimeManager.Instance.ChangeTimeScale(index);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        _tabMenu.SetActive(false);

        Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pauseMenu.activeSelf;

        if (pauseMenu.activeSelf)
            CameraController.Instance.PlayerInput.currentActionMap.Disable();
        else
            CameraController.Instance.PlayerInput.currentActionMap.Enable();

        if (planeetIsSelected == true)
        {
            landenOpPlaneet.SetActive(!landenOpPlaneet.activeSelf);
        }
        if (opPlaneet == true)
        {
            verlaatPlaneet.SetActive(!verlaatPlaneet.activeSelf);
        }
    }

    /// <summary>
    /// Toggles the visibility of the tab menu.
    /// If the pause menu is active, it will not toggle the tab menu.
    /// </summary>
    /// <param name="context"></param>
    private void OnTab(InputAction.CallbackContext context)
    {
        if (pauseMenu.activeSelf)
            return;

        _tabMenu.SetActive(!_tabMenu.activeSelf);

        if (_tabMenu.activeSelf)
            CameraController.Instance.PlayerInput.currentActionMap.Disable();
        else
            CameraController.Instance.PlayerInput.currentActionMap.Enable();

        Cursor.lockState = _tabMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _tabMenu.activeSelf;
    }

    public void ShowPopup(string header, string content)
    {
        _popupPanel.SetActive(true);

        TextWriter.Instance.WriteText(content, header);
    }

    public void HidePopup()
    {
        _popupPanel.SetActive(false);

        TextWriter.Instance.RemoveText();
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
        PlaneetAfstandVerandering.Instance.ToggleModeSimulationMode();
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
