using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class HUD : MonoBehaviour
{
    [SerializeField] private Sprite pressedE;
    [SerializeField] private Sprite normalE;

    [SerializeField] private Sprite pressedF;
    [SerializeField] private Sprite normalF;
    
    [SerializeField] private Sprite bookOpen;
    [SerializeField] private Sprite bookClosed;

    // Start is called before the first frame update
    private VisualElement _rootVisualElement;
    
    private bool isBookOpen = false;
    private float time;

    void Start()
    {
        _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        var book = _rootVisualElement.Q<Button>("book");
        book.clicked += () =>
        {
            isBookOpen = !isBookOpen;
        };
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        var milliseconds = (int) (time * 100) % 100;
        var seconds = (int) time % 60;
        var minutes = (int) time / 60;
        var timeString = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        var timerElement = _rootVisualElement.Q<Label>("timer");
        timerElement.text = timeString;
        
        var keyE = _rootVisualElement.Q<VisualElement>("key0");
        var keyF = _rootVisualElement.Q<VisualElement>("key1");
        
        
        var targetE = Input.GetKey(KeyCode.E) ? pressedE : normalE;
        var targetF = Input.GetKey(KeyCode.F) ? pressedF : normalF;

        keyE.style.backgroundImage = new StyleBackground(targetE);
        keyF.style.backgroundImage = new StyleBackground(targetF);
        
        var targetBook = isBookOpen ? bookOpen : bookClosed;
        var book = _rootVisualElement.Q<Button>("book");
        book.style.backgroundImage = new StyleBackground(targetBook);
        book.clicked += () =>
        {
            isBookOpen = !isBookOpen;
        };
    }
}