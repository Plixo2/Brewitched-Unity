using System.Collections;
using System.Collections.Generic;
using assets.code;
using UnityEngine;

public class Potions : MonoBehaviour
{
    // Start is called before the first frame update
    private enum PotionType
    {
        double_jump,
        fire_resistance,
        jesus,
        dash,
        reverse
    }

    [SerializeField] private Player player;
    [SerializeField] private PotionType potionType;
    [SerializeField] private float potionDuration = 5f;

    private bool potionEnabled = false;
    [SerializeField] private float potionTimer;
    void Start()
    {
        potionTimer += potionDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (potionEnabled)
            {
                potionTimer -= Time.deltaTime;

                if (potionTimer <= 0.0f)
                {   
                    // switch (potionType)
                    // {
                    //     case PotionType.double_jump:
                    //     {
                    //         player.doubleJumpEnabled = false;
                    //         break;
                    //     }
                    //     case PotionType.fire_resistance:
                    //     {
                    //         player.fireResistanceEnabled = false;
                    //         break;
                    //     }
                    //     case  PotionType.jesus:
                    //     {
                    //         player.jesusPotionEnabled = false;
                    //         player.waterCollider.enabled = false;
                    //         break;
                    //     }
                    // }
                    potionEnabled = false;
                    Destroy(this.gameObject);
                }
            }
    }

    public void EnablePotion()
    {
        switch (potionType)
            {
                case PotionType.double_jump:
                {
                    StartCoroutine(EnableDoubleJump());
                    break;
                }
                case PotionType.fire_resistance:
                {
                    StartCoroutine(EnableFireResistance());
                    break;
                }
                case  PotionType.jesus:
                {
                    StartCoroutine(EnableJesusPotion());
                    break;
                }
                case PotionType.dash:
                {
                    StartCoroutine(EnableDashPotion());
                    break;
                }
            }
            potionEnabled = true;
    }
    public IEnumerator EnableDoubleJump()
        {
            player.doubleJumpEnabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.doubleJumpEnabled = false;
        }
    public IEnumerator EnableFireResistance()
    {
        player.fireResistanceEnabled = true;
        yield return new WaitForSecondsRealtime(potionDuration);
        player.fireResistanceEnabled = false;
    }
    public IEnumerator EnableJesusPotion()
        {
            player.jesusPotionEnabled = true;
            player.waterCollider.enabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.jesusPotionEnabled = false;
            player.waterCollider.enabled = false;
        }
    public IEnumerator EnableDashPotion()
        {
            player.dashPotionEnabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.dashPotionEnabled = false;
        }
}