using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private DropdownField _levelSelect;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _levelSelect = root.Q<DropdownField>("LevelSelect");
        List<string> levels = new List<string>();
        for (int i = 1; i <= LevelManager.GetLevelCount(); i++)
        {
            levels.Add(i.ToString());
        }
        _levelSelect.choices = levels;
        _levelSelect.value = _levelSelect.choices[0];
        LevelManager.SetLevel(int.Parse(_levelSelect.choices[0]) - 1);

        _levelSelect.RegisterValueChangedCallback(level => OnSelectedLevelChanged(level.newValue));

        var _startButton = root.Q<Button>("Start");
        _startButton.clicked += OnStartButtonClicked;

        var _exitButton = root.Q<Button>("Exit");
        _exitButton.clicked += OnExitButtonClicked;

       var _configButton = root.Q<Button>("Config");
       _configButton.clicked += OnConfigButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        print("Scene");
        SceneManager.LoadScene(LevelManager.GetSelectedSceneName(), LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    private void OnExitButtonClicked()
    {
        print("Quit");
        Application.Quit();
    }

    private void OnConfigButtonClicked()
    {
        print("TODO config");
    }

    private void OnSelectedLevelChanged(string level)
    {
        LevelManager.SetLevel(int.Parse(level) - 1);
    }

    // Update is called once per frame
    void Update()
    {
    }
}