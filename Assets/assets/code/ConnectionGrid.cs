using UnityEngine;

namespace assets.code
{
    /// <summary>
    ///     Spawns a grid of connection points at the start of the Game
    /// </summary>
    public class ConnectionGrid : MonoBehaviour
    {
        [SerializeField] private GameObject pointPrefab;

        [SerializeField] [Range(1, 10)] private int rows = 1;
        [SerializeField] [Range(1, 10)] private int collums = 1;
        [SerializeField] private float spacingX = 1;
        [SerializeField] private float spacingY = 1;
        [SerializeField] private bool generate = true;
        [SerializeField] private bool drawGizmos;


        private void Update()
        {
            if (generate)
            {
                generate = false;
                GeneratePoint();
            }
        }

        /// <summary>
        ///     debug drawing
        /// </summary>
        private void OnDrawGizmos()
        {
            if (drawGizmos) OnDrawGizmosSelected();
        }

        /// <summary>
        ///     debug drawing if the GameObject is selected
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 0, 0, 0.7f);
            var startPosition = transform.position;
            for (var x = 0; x < rows; x++)
            for (var y = 0; y < collums; y++)
            {
                var pos = startPosition + new Vector3(x * spacingX, y * spacingY, 0);
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }

        /// <summary>
        ///     Main generation function
        /// </summary>
        private void GeneratePoint()
        {
            if (pointPrefab == null) return;
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
                DestroyImmediate(child);
            }

            var startPosition = transform.position;
            for (var x = 0; x < rows; x++)
            for (var y = 0; y < collums; y++)
            {
                var pos = startPosition + new Vector3(x * spacingX, y * spacingY, 0);
                CreateConnection(pos);
            }
        }

        /// <summary>
        ///     Spawns a Connection point at a position
        /// </summary>
        /// <param name="position">position to spawn a point</param>
        private void CreateConnection(Vector3 position)
        {
            var point = Instantiate(pointPrefab, transform);
            point.transform.position = position;
        }
    }
}