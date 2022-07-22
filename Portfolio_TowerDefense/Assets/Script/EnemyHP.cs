using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;            // �ִ� ü��
    private float currentHP;        // ���� ü��

    private bool isDie = false;     // ���� ��� ���¸� true�� ����

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
        // ���� ü���� ���� ��Ȳ���� ���� Ÿ���� ����(damage) �� ������ enemy.OnDie() �� ������ ����� �� ����
        if (isDie == true) return;

        // ���� ü���� damage ��ŭ ����
        currentHP -= damage;

        StopCoroutine(HitAlphaAnimation());
        StartCoroutine(HitAlphaAnimation());

        if(currentHP <= 0)
        {
            isDie = true;
            // ���
            enemy.OnDie(EnemyDestroyType.Kill);
        }
    }

    // �� ���ݹ��� �� ���� ����
    IEnumerator HitAlphaAnimation()
    {
        Color color = spriteRenderer.color;

        // ���� 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.05f);

        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
