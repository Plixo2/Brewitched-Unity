using assets.code;
using UnityEngine;

namespace assets.images.mage2
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private float veloThreshold = 0.25f; 

        [SerializeField] private Animator animator;

        private bool rising = false;
        // Start is called before the first frame update
        void Start()
        {
            this._player = this.GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            var velocity = (this._player.fixedPosition - this._player.lastFixedPosition).y;
            var isGrounded = this._player.IsGrounded();
            var pressingSneak = Input.GetKey(KeyCode.S);
            if (isGrounded)
            {
                if (pressingSneak)
                {
                    //  animator.Play("Base Layer.jump 0", 0);
                }
                else
                {
                    //  animator.Play("Base Layer.idle", 0);
                }
            }

            if (animator != null)
            {
                animator.SetBool("Grounded", isGrounded);
                animator.SetBool("Sneaking", pressingSneak);
                animator.SetBool("Falling", velocity < -veloThreshold);
                animator.SetBool("Rising", rising);
            }
            if (velocity < -veloThreshold)
            {
                rising = false;
            }
        }

        public void OnJump()
        {
            if (animator != null)
            {
                rising = true;
                 animator.SetTrigger("OnJump");
              animator.Play("Base Layer.idle", 0);
            }
        }
    }
}