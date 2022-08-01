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

        //�̵���� ���� ���������� �� ���� ��ȯ
        Vector2 frontvec = new Vector2(rigid.position.x + nextMove*0.2f ,rigid.position.y);
        Debug.DrawRay(frontvec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontvec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke(); //���� �۵����� ��� Invoke�Լ� ����
            Invoke("Think", 5);
        }
    }

    void Think()
    {
        //-1~1������ ���� �� ����
        nextMove = Random.Range(-1, 2);
        float nextThinkTime = Random.Range(2f, 6f);

        //�־��� �ð� �� �Լ� ����
        Invoke("Think", nextThinkTime);

        //��������Ʈ �ִϸ��̼�
        anim.SetInteger("walkSpeed", nextMove);

        //�¿� ���� ����
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }
    }
}
