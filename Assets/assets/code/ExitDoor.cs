using UnityEditor.VersionControl;
using UnityEngine;

namespace assets.code
{
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] Animator animator;

        bool isOpen = false;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var shouldOpen = States.allValvesClosedOnce;
            if (shouldOpen && !isOpen)
            {
                animator.SetTrigger("open");
                Disable();
                isOpen = true;
            }
        }

        void Disable()
        {
            foreach (var componentsInChild in GetComponentsInChildren<Collider2D>())
            {
                componentsInChild.enabled = false;
            }
        }
    }
}