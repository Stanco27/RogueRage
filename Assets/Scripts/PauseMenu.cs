using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pausePanel;

    [Header("Input System")]
    public InputActionAsset inputActionAsset;
    private InputAction pauseAction;

    private const string MAIN_MENU_SCENE = "MainMenu";
    private bool isPaused = false;

    private void Awake()
    {
        var actionMap = inputActionAsset.FindActionMap("Player");
        if (actionMap != null)
        {
            pauseAction = actionMap.FindAction("Pause/Resume");
        }

        if (pauseAction == null)
        {
            Debug.LogError("Pause action not found in the Input Action Asset. Check names.", this);
        }
    }

    private void OnEnable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed += TogglePause;
            pauseAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= TogglePause;
            pauseAction.Disable();
        }
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
