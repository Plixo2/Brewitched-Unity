using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private Button _startButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _startButton =  root.Q<Button>("StartButton");

        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
