using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
    public bool MoveDir = false;

    PhotonView view;

    public float bulletSpeed;
    public float DestroyTime;
    public float knockForce;

    private void Start() {
        view = GetComponent<PhotonView>();
    }

    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(DestroyTime);
        this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void ChangeDir_left()
    {
        MoveDir = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void Update() 
    {
        if(!MoveDir)
        {
            transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
        }else{
            transform.Translate(Vector2.left * bulletSpeed * Time.deltaTime);
        }

    }

     private void OnTriggerEnter2D(Collider2D other) {

         if (!view.IsMine)
         {
             return;
         }

         PhotonView target = other.gameObject.GetComponent<PhotonView>();

         if(target != null && (!target.IsMine || target.IsRoomView))
         {
             if(target.tag == "Player")
             {
                 if(MoveDir)
                 {
                     knockForce = -5f;
                 }
                 if(!MoveDir)
                 {
                     knockForce = 5f;
                 }
                 target.RPC("KnockBack", RpcTarget.AllBuffered, knockForce);
             }
             this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
         }

        
    }
}
