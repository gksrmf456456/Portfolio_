using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement movement;
    private Transform target;
    private float damage;

    public void Setup(Transform target, float damage)
    {
        movement = GetComponent<Movement>();
        this.target = target;       // Ÿ���� �������� target
        this.damage = damage;
    }

    private void Update()
    {
        if (target != null) // target �� �����ϸ�
        {
            // �߻�ü�� target �� ��ġ�� �̵� 
            Vector3 direction = (target.position - transform.position).normalized;
            movement.MoveTo(direction);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;     // ���� �ƴ� ���� �ε�����
        if (collision.transform != target) return;      // ���� target�� ���� �ƴҶ�

        collision.GetComponent<EnemyHP>().TakeDamage(damage); // �� ü���� damage ��ŭ ����
        Destroy(this.gameObject);                       // �߻�ü ������Ʈ ����
    }
}
