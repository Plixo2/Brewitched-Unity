using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace assets.code
{
    /// <summary>
    /// follows the Camera, specified in the 'target' field.
    /// </summary>
    public class CamFollow : MonoBehaviour
    {
        [SerializeField] private Player target;
        [SerializeField] public Vector3 offset = new Vector3(0, 0, 0);
        [SerializeField] private float damping = 0.5f;
        [SerializeField] private bool onlyY = false;
        [SerializeField] private int groundY;
        [SerializeField] private int ceilingY;
        [SerializeField] private float deadZone = 0.5f;
        [SerializeField] public float cameraRaiseAmount = 0;
        [SerializeField] public float cameraLowerAmount = 0;

        [HideInInspector] public bool cameraRaised = false;
        [HideInInspector] public bool cameraLowered = false;

        private Vector3 _velocity = Vector3.zero;
       [HideInInspector] public Camera camera;

        private void Start()
        {
            camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            var cameraScale = camera.orthographicSize;
            var pos = target.lastGroundedPosition + offset;
            var currentPos = transform.position;

            var diff = pos - currentPos;
            var horizontalDistance = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
            if (horizontalDistance < deadZone)
            {
                pos = currentPos;
            }

            var posTemp = pos;
            if (posTemp.y - groundY <= cameraScale) // if below ground would be visible
            {
                if (!cameraRaised)
                {
                    pos.y = groundY + cameraScale; // show ground at the lowest tilelevel on screen
                }
                else
                {
                    pos.y = currentPos.y;
                }
            }
            else if (ceilingY - posTemp.y <= cameraScale) // if above ceiling would be visible
            {
                if (!cameraLowered)
                {
                    pos.y = ceilingY - 6; // show ceiling at the highest tilelevel on screen
                }
                else
                {
                    pos.y = currentPos.y;
                }
            }

            if (onlyY)
            {
                pos.x = currentPos.x;
                pos.z = currentPos.z;
            }

            transform.position = Vector3.SmoothDamp(currentPos, pos, ref _velocity, damping);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, deadZone);
        }
    }
}