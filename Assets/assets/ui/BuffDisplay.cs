using assets.code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuffDisplay : MonoBehaviour
{
    private VisualElement _rootVisualElement;

    public Sprite doubleJumpPotion;
    public Sprite fireResistancePotion;
    public Sprite jesusPotion;
    public Sprite dashPotion;
    public Sprite reverseTimePotion;

    public Player player;

    public List<VisualElement> buffs = new List<VisualElement>();

    // Start is called before the first frame update
    void Start()
    {
        _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 1; i <= 5; i++)
        {
            buffs.Add(_rootVisualElement.Q<VisualElement>("buff-" + i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        var activeBuffs = player.activePotions;

        for (int i = 0; i < buffs.Count; i++)
        {
            if(i < activeBuffs.Count)
            {
                buffs[i].style.display = DisplayStyle.Flex;
                int elementNumber = i + 1;
                VisualElement currentBuffSprite = buffs[i].Q<VisualElement>("buff-image-" + elementNumber);
                currentBuffSprite.style.backgroundImage = new StyleBackground(getBuffSprite(activeBuffs[i].potionType));

                Label currentBuffTimer = buffs[i].Q<Label>("buff-timer-" + elementNumber);
                var remainingDuration = activeBuffs[i].remainingDuration;
                var seconds = (int)remainingDuration;
                currentBuffTimer.text = seconds + "s";
            } 
            else
            {
                buffs[i].style.display = DisplayStyle.None;
            }
        }
    }

    Sprite getBuffSprite(Potions.PotionType type)
    {
        switch (type)
        {
            case Potions.PotionType.double_jump:
                return doubleJumpPotion;
            case Potions.PotionType.fire_resistance:
                return fireResistancePotion;
            case Potions.PotionType.jesus:
                return jesusPotion;
            case Potions.PotionType.dash:
                return dashPotion;
            case Potions.PotionType.reverse:
                return reverseTimePotion;
            default:
                return doubleJumpPotion;
        }
    }
}
