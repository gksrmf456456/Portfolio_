using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;            // 최대 체력
    private float currentHP;        // 현재 체력

    private bool isDie = false;     // 적이 사망 상태면 true로 설정

    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // 적의 체력이 죽을 상황에도 여러 타워의 공격(damage) 을 받으면 enemy.OnDie() 가 여러번 실행될 수 있음
        if (isDie == true) return;

        // 현재 체력을 damage 만큼 감소
        currentHP -= damage;

        StopCoroutine(HitAlphaAnimation());
        StartCoroutine(HitAlphaAnimation());

        if(currentHP <= 0)
        {
            isDie = true;
            // 사망
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    // 적 공격받을 때 투명도 조정
    IEnumerator HitAlphaAnimation()
    {
        Color color = spriteRenderer.color;

        // 투명도 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
