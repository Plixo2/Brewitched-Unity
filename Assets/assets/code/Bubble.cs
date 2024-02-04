using TMPro;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float range = 5;
    public float animationSpeed = 5f;
    public bool delete = true;
    public bool isRecipe;

    [TextAreaAttribute] public string text;

    [SerializeField] private GameObject player;
    private bool isDeleting;

    private TMP_Text textMeshPro;

    private bool wasInRange;


    // Start is called before the first frame update
    private void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null) textMeshPro.text = text;
    }

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            var inrange = distance <= range;
            if (isRecipe)
            {
                transform.localScale = Vector3.one;
                if (inrange)
                {
                    GetComponentInChildren<Animator>().enabled = true;
                    foreach (var componentsInChild in GetComponentsInChildren<SpriteRenderer>())
                        componentsInChild.enabled = true;
                }

                return;
            }

            if (inrange)
            {
                wasInRange = true;
            }
            else if (wasInRange && delete)
            {
                isDeleting = true;
                Destroy(gameObject, 10);
            }

            var target = inrange && !isDeleting ? Vector3.one : Vector3.zero;

            transform.localScale = Vector3.MoveTowards(transform.localScale, target,
                Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}