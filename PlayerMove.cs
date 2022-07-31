using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid.freezeRotation = true;
    }

    void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping",true);
        }
        //공기저항
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2 (rigid.velocity.x*0.3f, rigid.velocity.y);
        }
        //걸어갈 시 방향 전환
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //일정 속도 이하로 내려갈 시 걷는 모션 종료
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
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
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//플레이어 위치에서 아래로 선을 그림(게임에선 안보임)
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));//Layer의 이름이 Platform인 것만 스캔
            if (rayHit.collider != null)
            {
                Debug.Log(rayHit.distance);
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    
                } //ray가 닿았을 때 거리가 0.5이하일 때
            }
        }
    }
}
