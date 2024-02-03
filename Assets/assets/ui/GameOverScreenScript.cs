using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverScreenScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("retry-btn").clicked += () => RestartGame();
        root.Q<Button>("main-menu-btn").clicked += () => BackToMainMenu();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
