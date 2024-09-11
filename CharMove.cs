using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    // ���� ���� �Ÿ�
    private float leftMaxPos = 6.5f;
    // ������ ���� �Ÿ�
    private float rightMaxPos = 43.0f;
    // ���� ����
    private Vector3 wayPoint;
    // �ʱ� ���� ����
    private Vector2 starPostion;
    // ù ���������� üũ
    private bool startMove;
    // �̵� �ӵ�
    private float moveSpeed = 2f;
    // ���ʷ� �浹�� AI�� ������ ����
    private GameObject firstCollider;
    // �۾��� �̹� ó�� �Ǿ����� ���θ� ������ ����
    private bool hasProcessed;

    private void OnEnable()
    {
        transform.position = new Vector3(20, -4, 0);
        startMove = true;
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (startMove)
        {
            startMove = false;

            // 0 or 1 �� �� �ϳ��� �ޱ�
            int num = Random.Range(0, 1);

            // 0�̸� ���� ����, 1�̸� ������ ����
            if (num == 0)
            {
                wayPoint = Vector3.left;
            }

            else
            {
                wayPoint = Vector3.right;
            }
        }

        transform.position += wayPoint * moveSpeed * Time.deltaTime;

        if(transform.position.x >= rightMaxPos)
        {
            wayPoint = Vector3.left;
        }

        else if (transform.position.x <= leftMaxPos)
        {
            wayPoint = Vector3.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BonusGame bonusGame = InGameManager.instance.BonusGame;

        // �ٸ� AI�� �浹 ����
        if (collision.CompareTag("AI") && collision.gameObject.name == gameObject.name)
        {
            Debug.Log("AI triggered: " + collision.gameObject.name);
            CharMove otherMove = collision.gameObject.GetComponent<CharMove>();

            if(firstCollider == null)
            {
                // ���� �浹 AI ���
                firstCollider = collision.gameObject;
                // �ٸ� AI���� ������ ���� �浹 ���
                otherMove.firstCollider = gameObject;
            }

             if (collision.gameObject == firstCollider && !hasProcessed)
            {
                hasProcessed = true;

                if(bonusGame.charList.Contains(otherMove.firstCollider) == false)
                {
                    bonusGame.charList.Add(otherMove.firstCollider);
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}