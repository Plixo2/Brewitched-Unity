using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private VisualElement pauseMenu;
    bool gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        pauseMenu = root.Q<VisualElement>("pause-menu");
        Button resume = root.Q<Button>("resume-btn");
        resume.clicked += () => ResumeGame();
        root.Q<Button>("main-menu-btn").clicked += () => BackToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenu.style.display = DisplayStyle.Flex;
    }

    private void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
        pauseMenu.style.display = DisplayStyle.None;
    } 

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
