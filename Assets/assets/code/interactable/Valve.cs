using assets.code;
using UnityEngine;

public class Valve : Interactable
{
    // public Transform _transform;
    public Animator Animator;
    public LevelCompleteScreenScript levelCompleteScript;

    public ParticleSystem
        _particleSystem; // Drag the particle system to be controlled by this valve in Unity Inspector.

    private bool isOpen = true;

    new public void Start()
    {
        States.AddValve(this);
        States.AddInteractable(this);
    }

    public void toggleValve()
    {
        Animator.SetTrigger("Toggle");
        // _transform.Rotate(0, 0, 90);
        //playRotateAnimation();   // To be coded later!!
        if (isOpen)
        {
            if (_particleSystem != null)
            {
                _particleSystem.gameObject.SetActive(false);
            }

            isOpen = !isOpen;
            onClosedAllValves();
        }
        else
        {
            if (_particleSystem != null)
            {
                _particleSystem.gameObject.SetActive(true);
            }

            isOpen = !isOpen;
        }
    }

    private void onClosedAllValves()
    {
        foreach (Valve valve in States.getValves())
        {
            if (valve.getOpen())
            {
                return;
            }
        }

        States.allValvesClosedOnce = true;
        print("Level Completed, All Valves Closed!!");

        levelCompleteScript.FinishLevel();
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