using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_manager : MonoBehaviour
{
    public float volume;
    public GameObject pauseMenu;
    public PlayerInput playerInput;
    public PlaneetInformatie planeetInformatie;
    public PlaneetAfstandVerandering planeetAfstandVerandering;

    public string selectedPlaneet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnEnable()
    {
       playerInput.actions.FindAction("PauseMenu").performed += OnPause;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void BackButton()
    {

    }
    public void ChangeVolume()
    {

    }
    public void ChangeTypingSpeed()
    {

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
    public void LandOpPlaneet()
    {
        SceneManager.LoadScene(1);
        //SceneManager.LoadScene(selectedPlaneet);
    }
}
