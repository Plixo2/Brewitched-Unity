using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
public class ShowItemName : MonoBehaviour
{
    [SerializeField] private float playerDistance = 2;
    [SerializeField] private Player player;
    void Start()
    {
        this.transform.position = player.transform.position + new Vector3(0,playerDistance,0);

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0,playerDistance,0);
    }
}
}