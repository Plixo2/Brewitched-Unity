using System.Collections;
using System.Collections.Generic;
using assets.code;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerPotionThrowing : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Player player;
    [SerializeField] private float forceStart;
    [SerializeField] private float forceLimit;
    [SerializeField] private float force;

    void Start(){
        force = forceStart;
    }

    void Update()
    {

        if(player.HasHandItem()){

            if(Input.GetMouseButton(0)){
                
                if(force <= forceLimit){
                    force += Time.deltaTime;
                }

            }

            if(Input.GetMouseButtonUp(0)){

                    Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0f;

                    Item handItem = player.GetHandItem();
                    throwItem(handItem, mousePosition);

                    force = forceStart;
                }
        }

    }
    private void throwItem(Item handItem, Vector3 mousePosition)
        {
            handItem.Disconnect();
            Vector3 playerPosition = player.transform.position;
            Vector3 finalForce = Vector3.Normalize(mousePosition - playerPosition) * force;    
            handItem.rigidbody.AddForce(finalForce, ForceMode2D.Impulse);
        }    
}   
