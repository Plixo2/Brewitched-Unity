using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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