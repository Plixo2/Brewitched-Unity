using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
    public class PlayerOneWayPlatform : MonoBehaviour
    {
        [SerializeField] private float disableCollisionTime = 0.5f; 
        private HashSet<GameObject> currentOneWayPlatforms = new HashSet<GameObject>();

        [SerializeField] private Collider2D playerCollider;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(DisableCollision());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
            {
                currentOneWayPlatforms.Add(collision.gameObject);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
            {
                currentOneWayPlatforms.Remove(collision.gameObject);
            }
        }

        private IEnumerator DisableCollision()
        {
            var plattforms = new List<Collider2D>();
            foreach (var gameObject in this.currentOneWayPlatforms)
            {
                plattforms.Add(gameObject.GetComponent<Collider2D>());
            }
            
            foreach (var plattform in plattforms)
            {
                Physics2D.IgnoreCollision(playerCollider, plattform);
            }
            
            yield return new WaitForSeconds(disableCollisionTime);
            foreach (var plattform in plattforms)
            {
                Physics2D.IgnoreCollision(playerCollider, plattform, false);
            }
        }
    }
}