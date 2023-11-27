using System.Collections;
using System.Collections.Generic;
using assets.code;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;
    public LogicScript logic;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            logic.gameOver();

        }
    }
}
