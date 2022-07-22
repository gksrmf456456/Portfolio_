using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;          // Ÿ�� ������ ���� ������
    public GameObject followTowerPrefab;    // �ӽ�Ÿ�� �긮�� 
    public Weapon[] weapon;                 // ������ Ÿ�� ����

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite;           // �������� Ÿ�� �̹���
        public float damage;            // ���ݷ�
        public float slow;              // ���� �ۼ�Ʈ
        public float buff;              // ���ݷ� ������
        public float rate;              // ���� �ӵ�
        public float range;             // ���� ����
        public int cost;                // �ʿ� ��� (0���� : �Ǽ� / 1~���� : ���׷��̵�)
        public int sell;                // Ÿ�� �Ǹ� �� ȹ�� ���
    }
}
