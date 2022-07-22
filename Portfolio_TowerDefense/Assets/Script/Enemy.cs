using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ���ݿ� ����Ͽ�����, �� ������ �����Ͽ����� �����ϱ� ����
public enum EnemyDestroyType { Kill = 0, Arrive}

public class Enemy : MonoBehaviour
{
    private int wayPointCount;           // �̵� ��� ����
    private Transform[] wayPoints;       // �̵� ��� ����
    private int currentIndex = 0;        // ���� ��ǥ ���� �ε���
    private Movement movement;           // ������Ʈ �̵� ����
    private EnemySpawner enemySpawner;   // ���� ������ ������ ���� �ʰ� EnemySpawner�� �˷��� ����
    [SerializeField]
    private int gold = 10;               // ��� �� ȹ�� ������ ���

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints)
    {
        movement = GetComponent<Movement>();
        this.enemySpawner = enemySpawner;

        // �� �̵� ��� WayPoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        // �� �̵�, ��ǥ ���� ���� �ڷ�ƾ �Լ�
        StartCoroutine(OnMove());
    }

    IEnumerator OnMove()
    {
        // ���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            // ���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 * movement.MoveSpeed ���� ���� �� if�� ����
            // movement.MoveSpeed �� ���� ������ �� �����ӿ� 0.02���� ������ �����̱� ������ if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� ����
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    void NextMoveTo()
    {
        // ���� �̵��� wayPoints�� ���� ���� ���
        if(currentIndex < wayPointCount - 1)
        {
            // ���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            // �̵� ���� ���� => ���� ��ǥ����(wayPoints)
            currentIndex ++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement.MoveTo(direction);
        }
        // ���� ��ġ�� ������ wayPoint �̸� �� ������Ʈ ����
        else
        {
            // �� �����Ͽ� ����ϸ� ��� ȹ�� �Ұ�
            gold = 0;
            OnDie(EnemyDestroyType.Arrive);
        }
    }

    public void OnDie(EnemyDestroyType type)
    {
        // enemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy() �� ���� ������� �ʰ�
        // enemySpawner���� ������ ������ �� �ʿ��� ó���� �ϵ��� DestroyEnemy() �Լ� ȣ��
        enemySpawner.DestroyEnemy(type, this, gold);
    }
}
