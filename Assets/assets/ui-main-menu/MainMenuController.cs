using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private DropdownField _levelSelect;
    private VisualElement _root;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;

        _levelSelect = _root.Q<DropdownField>("LevelSelect");
        List<string> levels = new List<string>();
        for (int i = 1; i <= LevelManager.GetLevelCount(); i++)
        {
            levels.Add(i.ToString());
        }
        _levelSelect.choices = levels;
        _levelSelect.value = _levelSelect.choices[0];
        LevelManager.SetLevel(int.Parse(_levelSelect.choices[0]) - 1);

        _levelSelect.RegisterValueChangedCallback(level => OnSelectedLevelChanged(level.newValue));

        var _startButton = _root.Q<Button>("Start");
        _startButton.clicked += OnStartButtonClicked;

        var _exitButton = _root.Q<Button>("Exit");
        _exitButton.clicked += OnExitButtonClicked;

       var _levelButton = _root.Q<Button>("Level");
        _levelButton.clicked += OnLevelButtonClicked;
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

    private void OnLevelButtonClicked()
    {
        var levelSelect = _root.Q<DropdownField>("LevelSelect");
        if(levelSelect.style.display == DisplayStyle.None)
        {
            levelSelect.style.display = DisplayStyle.Flex;
        }
        else
        {
            levelSelect.style.display = DisplayStyle.None;
        }
        
        print("LevelSelect");
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