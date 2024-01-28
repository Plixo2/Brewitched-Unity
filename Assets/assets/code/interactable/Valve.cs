using assets.code;
using UnityEngine;

public class Valve : Interactable
{
    public Transform _transform;
    public ParticleSystem _particleSystem; // Drag the particle system to be controlled by this valve in Unity Inspector.
    private bool isOpen = true;
    
    new public void Start()
    {
            print("Registering Valve");
            States.AddValve(this);
            print("Registering Interactable");
            States.AddInteractable(this);
    }
    public void toggleValve()
    {
        _transform.Rotate(0, 0, 90);
        //playRotateAnimation();   // To be coded later!!
        if(isOpen)
        {
            _particleSystem.enableEmission = false;
            isOpen = !isOpen;
            onClosedAllValves();
        }
        else
        {
            _particleSystem.enableEmission = true;
            isOpen = !isOpen;
        }
    }
    private void onClosedAllValves()
    {
        foreach(Valve valve in States.getValves())
        {
            if(valve.getOpen())
            {
                return;
            }
        }
        
        States.allValvesClosedOnce = true;
        print("Level Completed, All Valves Closed!!");

        /*

        You can code here what happens when all valves are closed.
        //unlockDoor();
        //completeLevel();
        
        */
    }

    public bool getOpen()
    {
        return isOpen;
    }
}
