using UnityEngine;

namespace assets.code
{
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private BoxCollider2D _collider;

        private bool isOpen;

        // Start is called before the first frame update
        private void Start()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            var shouldOpen = States.allValvesClosedOnce;
            if (shouldOpen && !isOpen)
            {
                animator.SetTrigger("open");
                isOpen = true;
                _collider.isTrigger = true;
            }
        }
    }
}