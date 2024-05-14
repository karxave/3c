using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    [SerializeField]
    private InputManager _inputManager;

    private void Start()
    {
        _inputManager.OnMainMenuInput += BackToMainMenu;
    }

    private void OnDestroy()
    {
        _inputManager.OnMainMenuInput -= BackToMainMenu;
    }
    private void BackToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None; // unlock cursor
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}
