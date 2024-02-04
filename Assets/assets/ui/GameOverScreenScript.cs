using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using assets.code;

public class GameOverScreenScript : MonoBehaviour
{
    private VisualElement gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("retry-btn").clicked += () => RestartGame();
        root.Q<Button>("main-menu-btn").clicked += () => BackToMainMenu();
        gameOverScreen = root.Q<VisualElement>("game-over-screen");
    }

    // Update is called once per frame
    void Update()
    {
        if (!States.GetPlayerAlive())
        {
            
            gameOverScreen.style.display = DisplayStyle.Flex;
        } else
        {
            gameOverScreen.style.display = DisplayStyle.None;
        }
    }

    private void RestartGame()
    {
        States.SetPlayerAlive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HUD.time = 0f;
    }

    private void BackToMainMenu()
    {
        States.SetPlayerAlive(true);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
