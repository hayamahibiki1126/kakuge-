using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]

public class FollowCamella : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject player2Obj;
    public float xc, yc, j;
    Rigidbody2D player;
    Transform playerTransform;
    Rigidbody2D player2;
    Transform playerTransform2;
    Rigidbody2D rb;
    void Start()
    {

        player2Obj = GameObject.FindGameObjectWithTag("Enemy");
        player2 = player2Obj.GetComponent<Rigidbody2D>();
        playerTransform2 = player2Obj.transform;
    }
    void LateUpdate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Rigidbody2D>();
        playerTransform = playerObj.transform;
        MoveCamera();
    }
    void MoveCamera()
    {
        var seq = DOTween.Sequence();
        xc = (playerTransform.position.x + playerTransform2.position.x) / 2;
        yc = (playerTransform.position.y + playerTransform2.position.y) / 2;
        if (xc >= 0.7f) xc = 0.7f;
        if (xc <= -0.2f) xc = -0.2f;
        if (yc <= -0.6f) yc = -0.6f;
        if (yc >= -0.45f) yc = -0.45f;
        //while(yc < transform.position.y)
        //{
        //    j -= 0.01f;
        //    seq.SetDelay(0.2f);
        //    seq.AppendCallback(() => transform.position = new Vector3(xc, transform.position.y + j, transform.position.z));
        //}
        //int wa = playerObj.GetComponent<Player1>().waza;
        transform.position = new Vector3(xc, yc, transform.position.z);
    }
}