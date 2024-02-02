using assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthDisplay : MonoBehaviour
{
    private VisualElement _rootVisualElement;

    public int health;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public List<VisualElement> hearts = new List<VisualElement>();

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        for(int i = 1; i <= 5; i++)
        {
            hearts.Add(_rootVisualElement.Q<VisualElement>("heart-" + i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        health = player.health;
        for (int i = 0; i < hearts.Count; i++)
        {
            if(i < health)
            {
                hearts[i].style.backgroundImage = new StyleBackground(fullHeart);
            } else
            {
                hearts[i].style.backgroundImage = new StyleBackground(emptyHeart);
            }
        }
    }
}
