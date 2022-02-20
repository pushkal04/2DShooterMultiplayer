using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 1f;
    [SerializeField] float cameraMoveDelay = 7f;
    Rigidbody2D cameraRb;
    void Start()
    {
        cameraRb = GetComponent<Rigidbody2D>();
        Invoke("moveCamera", cameraMoveDelay);

        
    }

    void moveCamera()
    {
        Vector2 cameraVel = new Vector2 (0f, cameraSpeed) ;
        cameraRb.velocity = cameraVel;
    }
}
