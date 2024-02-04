using System.Collections;
using assets.code;
using UnityEngine;

public class Potions : MonoBehaviour
{
    public enum PotionType
    {
        double_jump,
        fire_resistance,
        jesus,
        dash,
        reverse
    }

    [SerializeField] private Player player;
    [SerializeField] public PotionType potionType;
    [SerializeField] private float potionDuration = 5f;

    public float remainingDuration;

    private void Update()
    {
        remainingDuration -= Time.deltaTime;
    }

    /// <summary>
    ///     Start the coroutine for the potion Effect based on the enum type and destroys the gameObejct afterwards
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnablePotion()
    {
        remainingDuration = potionDuration;
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
            case PotionType.jesus:
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

        Destroy(gameObject);
    }

    private IEnumerator EnableDoubleJump()
    {
        player.doubleJumpEnabled = true;
        player.activePotions.Add(this);
        yield return new WaitForSecondsRealtime(potionDuration);
        player.doubleJumpEnabled = false;
        player.activePotions.Remove(this);
    }

    private IEnumerator EnableFireResistance()
    {
        player.fireResistanceEnabled = true;
        player.activePotions.Add(this);
        yield return new WaitForSecondsRealtime(potionDuration);
        player.fireResistanceEnabled = false;
        player.activePotions.Remove(this);
    }

    private IEnumerator EnableJesusPotion()
    {
        player.jesusPotionEnabled = true;
        player.waterCollider.enabled = true;
        player.activePotions.Add(this);
        yield return new WaitForSecondsRealtime(potionDuration);
        player.jesusPotionEnabled = false;
        player.waterCollider.enabled = false;
        player.activePotions.Remove(this);
    }

    private IEnumerator EnableDashPotion()
    {
        player.dashPotionEnabled = true;
        player.activePotions.Add(this);
        yield return new WaitForSecondsRealtime(potionDuration);
        player.dashPotionEnabled = false;
        player.activePotions.Remove(this);
    }

    private IEnumerator EnableReversePotion()
    {
        var waterManager = States.GetWaterManager();
        waterManager.timeScale = -1;
        player.activePotions.Add(this);
        yield return new WaitForSeconds(potionDuration);
        waterManager.timeScale = 1;
        player.activePotions.Remove(this);
    }
}