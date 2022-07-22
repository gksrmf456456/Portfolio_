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
        this.target = target;       // 타워가 설정해준 target
        this.damage = damage;
    }

    private void Update()
    {
        if (target != null) // target 이 존재하면
        {
            // 발사체를 target 의 위치로 이동 
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
        if (!collision.CompareTag("Enemy")) return;     // 적이 아닌 대상과 부딪히면
        if (collision.transform != target) return;      // 현재 target인 적이 아닐때

        collision.GetComponent<EnemyHP>().TakeDamage(damage); // 적 체력을 damage 만큼 감소
        Destroy(this.gameObject);                       // 발사체 오브젝트 삭제
    }
}
