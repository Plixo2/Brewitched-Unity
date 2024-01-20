using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using assets.code;
using Unity.VisualScripting;
using UnityEngine;

public class Potions : MonoBehaviour
{
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

    /// <summary>
    /// Start the coroutine for the potion Effect based on the enum type and destroys the gameObejct afterwards
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnablePotion()
    {
        switch (potionType)
            {
                case PotionType.double_jump:
                {
                    yield return StartCoroutine(EnableDoubleJump());
                    break;
                }
                case PotionType.fire_resistance:
                {
                    yield return StartCoroutine(EnableFireResistance());
                    break;
                }
                case  PotionType.jesus:
                {
                    yield return StartCoroutine(EnableJesusPotion());
                    break;
                }
                case PotionType.dash:
                {
                    yield return StartCoroutine(EnableDashPotion());
                    break;
                }
                case PotionType.reverse:
                {
                    yield return StartCoroutine(EnableReversePotion());
                    break;
                }
            }
            Destroy(this.gameObject);
    }
    private IEnumerator EnableDoubleJump()
        {
            player.doubleJumpEnabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.doubleJumpEnabled = false;
        }
    private IEnumerator EnableFireResistance()
    {
        player.fireResistanceEnabled = true;
        yield return new WaitForSecondsRealtime(potionDuration);
        player.fireResistanceEnabled = false;
    }
    private IEnumerator EnableJesusPotion()
        {
            player.jesusPotionEnabled = true;
            player.waterCollider.enabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.jesusPotionEnabled = false;
            player.waterCollider.enabled = false;
        }
    private IEnumerator EnableDashPotion()
        {
            player.dashPotionEnabled = true;
            yield return new WaitForSecondsRealtime(potionDuration);
            player.dashPotionEnabled = false;
        }
    private IEnumerator EnableReversePotion()
        {
            var waterManager = States.GetWaterManager();
            waterManager.timeScale = -1;
            yield return new WaitForSeconds(potionDuration);
            waterManager.timeScale = 1;
        }
}