using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;

    private Button _local;
    private Button _exit;
    private Button _level;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        VisualElement panel = _uiDocument.rootVisualElement.Q("MainMenu");

        if (panel != null)
        {
            _local = panel.Q<Button>("Local");
            _local?.RegisterCallback<ClickEvent>(LoadLobby);
            _exit = panel.Q<Button>("Exit");
            _exit.RegisterCallback<ClickEvent>(ExitGame);
        }
    }

    private void OnDisable()
    {
        _local?.UnregisterCallback<ClickEvent>(LoadLobby);
        _exit?.UnregisterCallback<ClickEvent>(ExitGame);
    }

    private void LoadLobby(ClickEvent evt)
    {
        SceneManager.LoadScene("Lobby");
    }

    private void ExitGame(ClickEvent evt)
    {
        Application.Quit();
    }
}
