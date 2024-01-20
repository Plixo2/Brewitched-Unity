using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

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
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
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

    // Update is called once per frame
    void Update()
    {
    }
}