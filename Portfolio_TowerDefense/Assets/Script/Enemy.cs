using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 공격에 사망하였는지, 골 지점에 도착하였는지 구분하기 위함
public enum EnemyDestroyType { Kill = 0, Arrive}

public class Enemy : MonoBehaviour
{
    private int wayPointCount;           // 이동 경로 갯수
    private Transform[] wayPoints;       // 이동 경로 정보
    private int currentIndex = 0;        // 현재 목표 지점 인덱스
    private Movement movement;           // 오브젝트 이동 제어
    private EnemySpawner enemySpawner;   // 적의 삭제를 본인이 하지 않고 EnemySpawner에 알려서 삭제
    [SerializeField]
    private int gold = 10;               // 사망 시 획득 가능한 골드

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement = GetComponent<Movement>();
        this.enemySpawner = enemySpawner;

        // 적 이동 경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동, 목표 지점 설적 코루틴 함수
        StartCoroutine(OnMove());
    }

    IEnumerator OnMove()
    {
        // 다음 이동 방향 설정
        NextMoveTo();

        while (true)
        {
            // 적의 현재위치와 목표위치의 거리가 0.02 * movement.MoveSpeed 보다 작을 때 if문 실행
            // movement.MoveSpeed 를 곱한 이유는 한 프레임에 0.02보다 빠르게 움직이기 때문에 if 조건문에 걸리지 않고 경로를 탈주하는 오브젝트가 발생할 수 있음
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    void NextMoveTo()
    {
        // 아직 이동할 wayPoints가 남아 있을 경우
        if(currentIndex < wayPointCount - 1)
        {
            // 적의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;
            // 이동 방향 설정 => 다음 목표지점(wayPoints)
            currentIndex ++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement.MoveTo(direction);
        }
        // 현재 위치가 마지막 wayPoint 이면 적 오브젝트 삭제
        else
        {
            // 골에 도착하여 사망하면 골드 획득 불가
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        // enemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy() 를 직접 사용하지 않고
        // enemySpawner에게 본인이 삭제될 때 필요한 처리를 하도록 DestroyEnemy() 함수 호출
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}
