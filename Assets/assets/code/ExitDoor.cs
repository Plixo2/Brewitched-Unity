using UnityEditor.VersionControl;
using UnityEngine;

namespace assets.code
{
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] private AudioSource? winGame;
        [SerializeField] Animator animator;

        bool isOpen = false;
        private BoxCollider2D _collider;

        // Start is called before the first frame update
        void Start()
        {
            _collider = this.GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            var shouldOpen = States.allValvesClosedOnce;
            if (shouldOpen && !isOpen)
            {
                animator.SetTrigger("open");
                isOpen = true;
                _collider.isTrigger = true;
                winGame.Play();
            }
        }

    }
}