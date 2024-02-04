using UnityEngine;

namespace assets.code
{
    public class ShowItemName : MonoBehaviour
    {
        [SerializeField] private Vector3 playerDistance;
        [SerializeField] private Player player;

        private void Update()
        {
            transform.position = player.transform.position + playerDistance;
        }
    }
}