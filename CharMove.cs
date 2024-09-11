using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    // 왼쪽 제한 거리
    private float leftMaxPos = 6.5f;
    // 오른쪽 제한 거리
    private float rightMaxPos = 43.0f;
    // 시작 방향
    private Vector3 wayPoint;
    // 초기 시작 지점
    private Vector2 starPostion;
    // 첫 움직임인지 체크
    private bool startMove;
    // 이동 속도
    private float moveSpeed = 2f;
    // 최초로 충돌한 AI를 저장할 변수
    private GameObject firstCollider;
    // 작업이 이미 처리 되었는지 여부를 저장할 변수
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

            // 0 or 1 값 중 하나를 받기
            int num = Random.Range(0, 1);

            // 0이면 왼쪽 시작, 1이면 오른쪽 시작
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

        // 다른 AI와 충돌 감지
        if (collision.CompareTag("AI") && collision.gameObject.name == gameObject.name)
        {
            Debug.Log("AI triggered: " + collision.gameObject.name);
            CharMove otherMove = collision.gameObject.GetComponent<CharMove>();

            if(firstCollider == null)
            {
                // 최초 충돌 AI 기록
                firstCollider = collision.gameObject;
                // 다른 AI에도 본인을 최초 충돌 기록
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