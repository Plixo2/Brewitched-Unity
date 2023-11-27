using UnityEngine;

namespace assets.code
{
    public class KillPlayer : MonoBehaviour
    {
        public LogicScript logic;
    
        void Start()
        {
            logic = GameObject.FindGameObjectWithTag("logic").GetComponent<LogicScript>();
        }

    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<Player>();
                player.Kill();
                logic.gameOver();
            }
        }
    }
}
