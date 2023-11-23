using assets.code;
using UnityEngine;

namespace Assets.assets.code
{
    public class Potion : Item
    {
        [SerializeField] public PotionType PotionType;
    }

    public enum PotionType
    {
        DoubleJump = 0,
        HighJump = 1
    }
}