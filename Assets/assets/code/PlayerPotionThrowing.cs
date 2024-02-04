using assets.code;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPotionThrowing : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private float forceStart;
    [SerializeField] private float forceLimit;
    [SerializeField] private float force;
    [SerializeField] private float chargeSpeed;
    [SerializeField] public Image progressBarImage;
    [SerializeField] public Image fillImage;


    private void Start()
    {
        force = forceStart;
    }

    private void Update()
    {
        if (player.HasHandItem())
        {
            if (Input.GetMouseButton(0))
            {
                var fillImageScale = (force - forceStart) / (forceLimit - forceStart);
                progressBarImage.enabled = true;
                fillImage.enabled = true;
                fillImage.rectTransform.transform.localScale = new Vector3(fillImageScale, 1, 1);

                if (force <= forceLimit) force += Time.deltaTime * chargeSpeed;
            }

            if (Input.GetMouseButtonUp(0))
            {
                progressBarImage.enabled = false;
                fillImage.enabled = false;

                var handItem = player.GetHandItem();
                throwItem(handItem);

                force = forceStart;
            }
        }
    }

    private void throwItem(Item handItem)
    {
        handItem.Disconnect();

        var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        var playerPosition = player.transform.position;
        var finalForce = Vector3.Normalize(mousePosition - playerPosition) * force;
        handItem.rigidbody.AddForce(finalForce, ForceMode2D.Impulse);
    }
}