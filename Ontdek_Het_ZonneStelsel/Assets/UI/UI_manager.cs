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
    public GameObject landenOpPlaneet;
    public GameObject verlaatPlaneet;
    public bool opPlaneet = false;
    public bool planeetIsSelected = false;

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

        if (planeetIsSelected == true)
        {
            landenOpPlaneet.SetActive(!landenOpPlaneet.activeSelf);
        }
        if (opPlaneet == true)
        {
            verlaatPlaneet.SetActive(!verlaatPlaneet.activeSelf);
        }
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
    public void LandOpPlaneet(string selectedPlanet)
    {
        SceneManager.LoadScene(selectedPlanet);
        //SceneManager.LoadScene(selectedPlanet);
    }
    public void VerlaatPlaneet()
    {
        SceneManager.LoadScene("Zonnestelsel");
    }
}
