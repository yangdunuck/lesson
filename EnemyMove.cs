using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();
        rigid.freezeRotation = true;
    }
    void FixedUpdate()
    {
        rigid.velocity = new Vector2 (nextMove , rigid.velocity.y);

        //이동경로 앞이 낭떠러지일 때 방향 전환
        Vector2 frontvec = new Vector2(rigid.position.x + nextMove*0.2f ,rigid.position.y);
        Debug.DrawRay(frontvec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontvec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke(); //현재 작동중인 모든 Invoke함수 정지
            Invoke("Think", 5);
        }
    }

    void Think()
    {
        //-1~1까지의 랜덤 수 생성
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 6f);

        //주어진 시간 후 함수 실행
        Invoke("Think", nextThinkTime);

        //스프라이트 애니메이션
        anim.SetInteger("walkSpeed", nextMove);

        //좌우 방향 반전
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }
    }
}
