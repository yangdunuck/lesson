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
        //����
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))//JumpŰ�� ������, Jump����� ���� ���� �ƴ� �� ����
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping",true);
        }
        //��������
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2 (rigid.velocity.x*0.3f, rigid.velocity.y);
        }

        //�ɾ �� ���� ��ȯ
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //���� �ӵ� ���Ϸ� ������ �� �ȴ� ��� ����
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Ű���带 �̿��Ͽ� �̵�
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed) //������ �������� �ְ�ӵ��� �Ѿ�� �� �ӵ��� �ְ�ӵ��� ����
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        
        else if (rigid.velocity.x < maxSpeed*(-1)) //���� �������� �ְ�ӵ��� �Ѿ�� �� �ӵ��� �ְ�ӵ��� ����
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));//�÷��̾� ��ġ���� �Ʒ��� ���� �׸�(���ӿ��� �Ⱥ���)
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));//Layer�� �̸��� Platform�� �͸� ��ĵ
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    
                } //ray�� ����� �� �Ÿ��� 0.5������ ��
            }
        }
    }

    //�ٸ� ������Ʈ�� �浹 �� ����
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamage(collision.transform.position);
        }
    }

    //�ǰ� �� ����
    void OnDamage(Vector2 targetPos)
    {
        //���� �浹���� �ʴ� ���̾�� ����
        gameObject.layer = 11;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f); //Color(R, G, B, ����)

        //�ǰ� �� �˹�
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        //�ִϸ��̼�
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 2);
    }

    //�����ð� ����
    void OffDamaged()
    {
        //���� �浹�ϴ� ���̾�� ����
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
