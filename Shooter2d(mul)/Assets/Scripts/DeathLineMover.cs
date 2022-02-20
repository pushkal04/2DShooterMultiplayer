using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLineMover : MonoBehaviour
{
    [SerializeField] float deathLineSpeed = 1f;
    [SerializeField] float deathLineMoveDelay = 5f;
    Rigidbody2D deathLineRb;

    void Start()
    {
        deathLineRb = GetComponent<Rigidbody2D>();
        Invoke("moveCamera", deathLineMoveDelay);
        
    }

    void moveCamera()
    {
        Vector2 deathLineVel = new Vector2 (0f, deathLineSpeed) ;
        deathLineRb.velocity = deathLineVel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(other.gameObject);
        }
        
    }
}
