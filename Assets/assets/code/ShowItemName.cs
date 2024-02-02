using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace assets.code
{
public class ShowItemName : MonoBehaviour
{
    [SerializeField] private Vector3 playerDistance;
    [SerializeField] private Player player;
    void Update()
    {
        this.transform.position = player.transform.position + playerDistance;
    }
}
}