using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class LevelCompleteScreenScript: MonoBehaviour
{
    private static VisualElement levelCompleteScreen;
    static Label completionTime;
    static Label successText;
    static Button continueButton;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        continueButton = root.Q<Button>("continue-btn");
        continueButton.clicked += () => PlayNextLevel();
        root.Q<Button>("main-menu-btn").clicked += () => BackToMainMenu();
        completionTime = root.Q<Label>("completion-time");
        successText = root.Q<Label>("success-text");

        levelCompleteScreen = root.Q<VisualElement>("level-complete-screen");
    }

    private static string getTimeString(float time)
    {
        var milliseconds = (int)(time * 100) % 100;
        var seconds = (int)time % 60;
        var minutes = (int)time / 60;
        return minutes + ":" + seconds + ":" + milliseconds;
    }

    public static void FinishLevel()
    {
        completionTime.text = getTimeString(HUD.time);

        if (LevelManager.WasLastLevel())
        {
            successText.text = "Gratulations you finally beat the last stage";
            continueButton.style.display = DisplayStyle.None;
        }

        Time.timeScale = 0f;
        levelCompleteScreen.style.display = DisplayStyle.Flex;
    }

    private void PlayNextLevel()
    {
        SceneManager.LoadScene(LevelManager.AdvanceLevel(), LoadSceneMode.Single);
        HUD.time = 0f;
        Time.timeScale = 1f;
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
