using System.Collections;
using System.Collections.Generic;
using assets.code;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
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


    void Start(){
        force = forceStart;
    }
    void Update()
    {
        if(player.HasHandItem()){

            if(Input.GetMouseButton(0)){
                
                float fillImageScale = (force - forceStart) / (forceLimit - forceStart);
                progressBarImage.enabled = true;
                fillImage.enabled = true;
                fillImage.rectTransform.transform.localScale = new Vector3(fillImageScale, 1, 1);
                
                if(force <= forceLimit){
                    force += Time.deltaTime * chargeSpeed;
                }
            }

            if(Input.GetMouseButtonUp(0)){
                    progressBarImage.enabled = false;
                    fillImage.enabled = false;
                    
                    Item handItem = player.GetHandItem();
                    throwItem(handItem);

                    force = forceStart;
                }
        }
    }
    private void throwItem(Item handItem)
        {
            handItem.Disconnect();

            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0f;
                    
            Vector3 playerPosition = player.transform.position;
            Vector3 finalForce = Vector3.Normalize(mousePosition - playerPosition) * force;    
            handItem.rigidbody.AddForce(finalForce, ForceMode2D.Impulse);
        }    
}   
