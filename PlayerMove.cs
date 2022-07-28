using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.freezeRotation = true;
    }

    void FixedUpdate()
    {
        //키보드를 이용하여 이동
        float h = Input.GetAxisRaw("Horizontal"); 
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) //오른쪽 방향으로 최고속도를 넘어섰을 때 속도를 최고속도로 고정
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        
        else if (rigid.velocity.x < maxSpeed*(-1)) //왼쪽 방향으로 최고속도를 넘어섰을 때 속도를 최고속도로 고정
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);


    }
}
