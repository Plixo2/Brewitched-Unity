using System;
using System.Collections;
using System.Collections.Generic;
using assets.code;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float range = 5;
    public float animationSpeed = 5f;

    [TextAreaAttribute] public string text;

    TMP_Text textMeshPro;

    [SerializeField] GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        textMeshPro.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            var distance = Vector3.Distance(player.transform.position, transform.position);
            var target = distance <= range ? Vector3.one : Vector3.zero;
            this.transform.localScale = Vector3.MoveTowards(this.transform.localScale, target,
                Time.deltaTime * animationSpeed);
        }
        else
        {
            this.transform.localScale = Vector3.one;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}