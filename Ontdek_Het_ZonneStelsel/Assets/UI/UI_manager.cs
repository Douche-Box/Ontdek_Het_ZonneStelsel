using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_manager : MonoBehaviour
{
    public float volume;
    public GameObject pauseMenu;

    public PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnEnable()
    {
        playerInput.actions.FindAction("PauseMenu").performed += _ => pauseMenu.SetActive(!pauseMenu.activeSelf);
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

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
