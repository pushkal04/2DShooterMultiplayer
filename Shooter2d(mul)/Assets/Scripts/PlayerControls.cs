using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    //Game values
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;

    public int TotalBullets = 5;

    public Text Myname;
    public GameObject BulletObject;
    public Transform Firepos;
    SpriteRenderer sr;

    //Character object reference
    PhotonView photonView;
    Rigidbody2D myRigidBody;
    BoxCollider2D myFeetCollider;
    Animator myAnimator;

    //Movement reference


    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        photonView = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();

        if(photonView.IsMine)
        {
            Myname.text = PhotonNetwork.NickName;

        }else{
           Myname.text = photonView.Owner.NickName;
        }


        
    }

    void Update()
    {
        if(photonView.IsMine)
        {
       
         CheckInput();
        }

         //Jumping animation
         if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))   
         {
            myAnimator.SetBool("isJumping", false);

         }

         if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
         {
             myAnimator.SetBool("isJumping", true);

         }



    }

    // void FixedUpdate()
    // {
    //     FlipSprite();
        
    // }

    // void Run()
    // {
    //     // Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed , myRigidBody.velocity.y);
    //     // myRigidBody.velocity = playerVelocity;
    //     transform.position += moveInput.x * moveSpeed * Time.deltaTime;

    //     bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    //     myAnimator.SetBool("isWalking", playerHasHorizontalSpeed);
    // }

    void CheckInput()
    {
        var moveInput = new Vector3(Input.GetAxisRaw("Horizontal"),0); //Returns 1,-1 or 0 for direction
       transform.position += moveInput * moveSpeed * Time.deltaTime;

       if(Input.GetKeyDown(KeyCode.A))
       {
           photonView.RPC("FlipTrue", RpcTarget.AllBuffered);
       }

       if(Input.GetKeyDown(KeyCode.D))
       {
           photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
       }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            myAnimator.SetBool("isWalking", true);
        }else{
            myAnimator.SetBool("isWalking", false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (TotalBullets != 0)
            {
                Shoot();
                TotalBullets -- ;
            }else if (TotalBullets == 0)
            {
                Invoke("reloadBullets", 5f);

            }
          
        }


    }

    private void reloadBullets()
    {
        TotalBullets = 5;
    }

    // void FlipSprite()
    // {
    //     bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    //     if(playerHasHorizontalSpeed)
    //     {

    //         if(Mathf.Sign(myRigidBody.velocity.x) < 0)
    //         {
    //             photonView.RPC("FlipTrue", RpcTarget.AllBuffered);

    //         }else if(Mathf.Sign(myRigidBody.velocity.x) > 0)
    //         {
    //             photonView.RPC("FlipFalse", RpcTarget.AllBuffered);
    //         }
    //     }
    // }

    private void Shoot()
    {
        if(sr.flipX == false)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(Firepos.transform.position.x, Firepos.transform.position.y), Quaternion.identity, 0);

        }

        if(sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(Firepos.transform.position.x, Firepos.transform.position.y), Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("ChangeDir_left", RpcTarget.AllBuffered);
        }
        
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }

     [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }

    void Jump()
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){return; }
        myRigidBody.velocity += new Vector2(0f, jumpForce);
    }

    [PunRPC] public void KnockBack(float knockForce)
    {
        myRigidBody.velocity += new Vector2 (knockForce , 0f);

    }
}
