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
        // Debug.Log(mainCamera.ScreenToWorldPoint(Input.mousePosition))

        if(player.HasHandItem()){

            if(Input.GetMouseButton(0)){
                
                // var hantItemPosition = handItem.transform.position;
                // hantItemPosition = Vector3.MoveTowards(hantItemPosition, mousePosition, speed * Time.deltaTime);
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
            handItem._connectionPoint = null;
            handItem.rigidbody.bodyType = RigidbodyType2D.Dynamic;

            Vector3 playerPosition = player.transform.position;

            Vector3 finalForce = Vector3.Normalize(mousePosition - playerPosition) * force;
            

            handItem.rigidbody.AddForce(finalForce, ForceMode2D.Impulse);
            
            handItem.transform.parent = null;
            handItem._spriteRenderer.sortingOrder = 20;
            handItem.transform.rotation = Quaternion.Euler(0, 0, 0);

            handItem.EnableColliders();
        }    
}   
